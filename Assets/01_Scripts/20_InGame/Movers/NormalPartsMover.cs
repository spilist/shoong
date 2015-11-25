using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  private NormalPartsManager npm;
  private MeshFilter filter;
  private Skill_Gold goldSkill;
  private bool popping;
  private Vector3 popDir;
  private int popDistance;
  private float curDistance;
  private Vector3 origin;

  protected override void initializeRest() {
    npm = (NormalPartsManager)objectsManager;
    filter = GetComponent<MeshFilter>();
    goldSkill = (Skill_Gold)SkillManager.sm.getSkill("Gold");
  }

  protected override void afterEnable() {
    filter.sharedMesh = npm.getRandomMesh();
    if (popping) {
      transform.localScale = npm.popStartScale * Vector3.one;
      shrinkedScale = npm.popStartScale;
    } else {
      GetComponent<Collider>().enabled = true;
    }
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

  public void pop(int popDistance) {
    curDistance = 0;
    this.popDistance = popDistance;
    Vector2 randomV = Random.insideUnitCircle.normalized;
    popDir = new Vector3(randomV.x, 0, randomV.y);
    origin = transform.position;
    popping = true;
    gameObject.SetActive(true);
  }

  void Update() {
    if (popping) {
      curDistance = Mathf.MoveTowards(curDistance, popDistance, Time.deltaTime * npm.poppingSpeed);
      transform.position = curDistance * popDir + origin;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, originalScale, Time.deltaTime * 10);
      transform.localScale = shrinkedScale * Vector3.one;

      if (curDistance >= popDistance && shrinkedScale >= originalScale) {
        popping = false;
        GetComponent<Collider>().enabled = true;
      }
    }
  }
}
