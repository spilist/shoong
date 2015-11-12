using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  private NormalPartsManager npm;
  private MeshFilter filter;
  private Skill_Gold goldSkill;

  protected override void initializeRest() {
    npm = (NormalPartsManager)objectsManager;
    filter = GetComponent<MeshFilter>();
    goldSkill = (Skill_Gold)SkillManager.sm.getSkill("Gold");
  }

  protected override void afterEnable() {
    filter.sharedMesh = npm.getRandomMesh();
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
}
