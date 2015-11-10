using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  // private NormalPartsManager npm;
  // private MeshFilter filter;
  private Material originalMaterial;
  private bool goldenTransformed;
  private Skill_Gold goldSkill;

  protected override void initializeRest() {
    // npm = (NormalPartsManager)objectsManager;
    // filter = GetComponent<MeshFilter>();
    goldSkill = (Skill_Gold)SkillManager.sm.getSkill("Gold");
    originalMaterial = GetComponent<Renderer>().sharedMaterial;
  }

  protected override void afterEnable() {
    // filter.sharedMesh = npm.getRandomMesh();
    goldenTransformed = false;
    GetComponent<Renderer>().sharedMaterial = originalMaterial;
  }

  override protected void afterEncounter() {
    if (isMagnetized) {
      DataManager.dm.increment("NumPartsAbsorbedWithBlackhole");
    }

    if (player.isUsingRainbow()) {
      DataManager.dm.increment("NumPartsGetOnRainbow");
    }

    if (goldenTransformed) {
      GoldManager.gm.add(transform.position);
    }
  }

  override public string getManager() {
    return "NormalPartsManager";
  }

  public void transformToGold(Vector3 pos) {
    if (goldenTransformed) return;

    GameObject laser = goldSkill.getLaser(pos);
    laser.SetActive(true);
    laser.GetComponent<TransformLaser>().shoot(transform.position, goldSkill.laserShootDuration);

    Invoke("changeToGold", goldSkill.laserShootDuration);
  }

  void changeToGold() {
    GetComponent<Renderer>().sharedMaterial = goldSkill.goldenPartMaterial;
    goldSkill.getParticle(transform.position);
    goldenTransformed = true;
  }
}
