using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  private NormalPartsManager npm;
  private MeshFilter filter;
  private Skill_Gold goldSkill;
  private bool popping;
  private bool runningToPlayer;
  private Vector3 popDir;
  private int popDistance;
  private float curDistance;
  private Vector3 origin;
  private Animation beatAnimation;

  private float sizeCoeff;
  private int divideCount;

  protected override void initializeRest() {
    npm = (NormalPartsManager)objectsManager;
    filter = GetComponent<MeshFilter>();
    goldSkill = (Skill_Gold)SkillManager.sm.getSkill("Gold");
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  protected override void afterEnable() {
    runningToPlayer = false;
    filter.sharedMesh = npm.getRandomMesh();
    if (popping) {
      transform.localScale = npm.popStartScale * Vector3.one;
      shrinkedScale = npm.popStartScale;
    } else {
      GetComponent<Collider>().enabled = true;
    }
  }

  public override void encounterPlayer(bool destroy = true) {
    // Need to check popping and runningtoplayer, because this is called a lot while the object is encountering player
    if (divideCount > 1 && !popping && !runningToPlayer) {
      npm.popSweets(divideCount, transform.position, true);
    }

    base.encounterPlayer(destroy);
  }

  override protected void afterEncounter() {
    if (isMagnetized) {
      DataManager.dm.increment("NumPartsAbsorbedWithBlackhole");
    }

    if (player.isUsingRainbow()) {
      DataManager.dm.increment("NumPartsGetOnRainbow");
    }
  }

  override public string getManager() {
    return "NormalPartsManager";
  }

  public void setSize(float sizeCoeff, int divideCount) {
    this.sizeCoeff = sizeCoeff;
    // Currently, only 1 and 5 are supported because of the impl of animation
    Animation anim = GetComponent<Animation>();
    anim.Stop();
    if (sizeCoeff == 1) {
      anim.clip = anim.GetClip("Candy");
    } else if (sizeCoeff == 5) {
      anim.clip = anim.GetClip("Candy_Big");
    }
    anim.Play();
    this.transform.localScale = Vector3.one * sizeCoeff;
    this.divideCount = divideCount;
  }

  public void transformToGold(Vector3 pos) {
    GameObject laser = goldSkill.getLaser(pos);
    laser.SetActive(true);
    laser.GetComponent<TransformLaser>().shoot(transform.position, goldSkill.laserShootDuration);

    Invoke("changeToGold", goldSkill.laserShootDuration);
  }

  void changeToGold() {
    destroyObject(false);
    goldSkill.getParticle(transform.position);
    npm.gcm.spawnGoldenCube(transform.position);
  }

  public IEnumerator pop(int popDistance, bool autoEatAfterPopping = false) {
    curDistance = 0;
    this.popDistance = popDistance;
    Vector2 randomV = Random.insideUnitCircle.normalized;
    popDir = new Vector3(randomV.x, 0, randomV.y);
    origin = transform.position;
    popping = true;
    gameObject.SetActive(true);
    yield return popUpdate();
    if (autoEatAfterPopping)
      yield return eatAfterPopping();
  }

  IEnumerator popUpdate() {
    while (popping) {
      curDistance = Mathf.MoveTowards(curDistance, popDistance, Time.deltaTime * npm.poppingSpeed);
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
}
