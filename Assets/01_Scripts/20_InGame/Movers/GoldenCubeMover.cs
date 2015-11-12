using UnityEngine;
using System.Collections;

public class GoldenCubeMover : ObjectsMover {
  private GoldenCubeManager gcm;
  private SummonPartsManager summonManager;
  private bool detected = false;
  private bool noRespawn = false;
  private Renderer mRenderer;

  protected override void initializeRest() {
    gcm = (GoldenCubeManager)objectsManager;
    mRenderer = GetComponent<Renderer>();
  }

  override protected void afterEnable() {
    detected = false;
    noRespawn = false;
    mRenderer.enabled = true;
  }

  public void setNoRespawn(bool autoDestroy) {
    noRespawn = true;
    if (autoDestroy) {
      summonManager = gcm.GetComponent<SummonPartsManager>();
      StartCoroutine("destroyAfter");
    }
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

  // protected override void normalMovement() {
  //   direction = player.transform.position - transform.position;
  //   float distance = direction.magnitude;
  //   direction /= distance;
  //   if (distance < gcm.detectDistance) {
  //     rb.velocity = -direction * gcm.speed;
  //     if (!detected) {
  //       detected = true;
  //     }
  //   }
  // }

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

  override public bool noCubesByDestroy() {
    return true;
  }

  override protected float getSpeed() {
    return 0;
  }
}
