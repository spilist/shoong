using UnityEngine;
using System.Collections;

public class GoldenCubeMover : ObjectsMover {
  private GoldenCubeManager gcm;
  private SummonPartsManager summonManager;
  private bool noRespawn = false;
  private Renderer mRenderer;
  private bool popping;
  private bool runningToPlayer;
  private Vector3 popDir;
  private int popDistance;
  private float curDistance;
  private Vector3 origin;

  protected override void initializeRest() {
    gcm = (GoldenCubeManager)objectsManager;
    mRenderer = GetComponent<Renderer>();
  }

  override protected void afterEnable() {
    runningToPlayer = false;
    noRespawn = false;
    mRenderer.enabled = true;

    if (popping) {
      transform.localScale = gcm.npm.popStartScale * Vector3.one;
      shrinkedScale = gcm.npm.popStartScale;
    } else {
      GetComponent<Collider>().enabled = true;
    }
  }

  public void setNoRespawn(bool autoDestroy) {
    noRespawn = true;
    if (autoDestroy) {
      summonManager = gcm.GetComponent<SummonPartsManager>();
      StartCoroutine("destroyAfter");
    }
  }

  public IEnumerator pop(int popDistance, bool autoEatAfterPopping = false) {
    curDistance = 0;
    this.popDistance = popDistance;
    Vector2 randomV = Random.insideUnitCircle.normalized;
    popDir = new Vector3(randomV.x, 0, randomV.y);
    origin = transform.position;
    popping = true;
    gameObject.SetActive(true);
    noRespawn = true;
    yield return popUpdate();
    if (autoEatAfterPopping)
      yield return eatAfterPopping();
  }

  IEnumerator popUpdate() {
    while (popping) {
      curDistance = Mathf.MoveTowards(curDistance, popDistance, Time.deltaTime * gcm.npm.poppingSpeed);
      transform.position = curDistance * popDir + origin;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, originalScale, Time.deltaTime * 10);
      transform.localScale = shrinkedScale * Vector3.one;

      if (curDistance >= popDistance && shrinkedScale >= originalScale) {
        popping = false;
        GetComponent<Collider>().enabled = true;
        transform.Find("PopAudio").GetComponent<AudioSource>().Play();
      }
      yield return null;
    }
  }

  IEnumerator eatAfterPopping() {
    runningToPlayer = true;
    while (runningToPlayer) {
      transform.position = Vector3.MoveTowards(transform.position, Player.pl.transform.position, Player.pl.speed * Time.deltaTime * 2);
      yield return null;
    }
  }

  void Update() {
    
  }

  IEnumerator destroyAfter() {
    yield return new WaitForSeconds(summonManager.summonedPartLifetime - summonManager.blinkBeforeDestroy);
    float duration = summonManager.blinkBeforeDestroy;
    float showDuring = summonManager.showDurationStart;
    float emptyDuring = summonManager.emptyDurationStart;
    float showDurationDecrease = summonManager.showDurationDecrease;
    float emptyDurationDecrease = summonManager.emptyDurationDecrease;

    while (duration > 0) {
      mRenderer.enabled = true;

      yield return new WaitForSeconds (showDuring);

      mRenderer.enabled = false;
      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring > 1f) showDuring -= showDurationDecrease;
      if(emptyDuring > 0.5f) emptyDuring -= emptyDurationDecrease;
    }

    destroyObject();
  }

  protected override bool beforeCollide(Collision collision) {
    if (collision.collider.tag == "CubeDispenser") {
      processCollision(collision);
      return false;
    } else {
      return true;
    }
  }

  override public void destroyObject(bool destroyEffect = true, bool byPlayer = false, bool respawn = true) {

    gameObject.SetActive(false);
    transform.parent = gcm.transform;

    if (destroyEffect) {
      showDestroyEffect(byPlayer);
    }

    if (noRespawn) return;

    if (byPlayer) {
      objectsManager.run();
    } else {
      objectsManager.runImmediately();
    }
  }

  override protected void afterEncounter() {
    transform.parent = gcm.transform;
    GoldManager.gm.add(transform.position, gcm.cubesWhenEncounter());

    if (noRespawn) return;

    objectsManager.run();
  }

  override public string getManager() {
    return "GoldenCubeManager";
  }

  override protected float getSpeed() {
    return 0;
  }

  override public int energyGets() {
    if (CharacterManager.cm.getCurrentCharacter() == "goldpig") {
      return objectsManager.energyGets;
    } else {
      return 0;
    }
  }
}
