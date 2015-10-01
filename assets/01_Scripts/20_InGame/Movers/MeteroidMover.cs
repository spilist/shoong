using UnityEngine;
using System.Collections;

public class MeteroidMover : ObjectsMover {
  private SpecialPartsManager spm;
  private bool avoiding = false;
  private bool alreadyChecked = false;

  override public string getManager() {
    return "MeteroidManager";
  }

  protected override void initializeRest() {
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();

    canBeMagnetized = false;
  }

  override protected void afterCollide(Collision collision) {
    if (QuestManager.qm.doingQuest("FallingStarReboundByCubeDispenser") && collision.collider.gameObject.tag == "CubeDispenser") {
      Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
      if (pos.x >= 0.0f && pos.x <= 1.0f && pos.y >= 0.0f && pos.y <= 1.0f) {
        QuestManager.qm.addCountToQuest("FallingStarReboundByCubeDispenser");
      }
    }
  }

  override protected void afterDestroy(bool byPlayer) {
    if (avoiding && !alreadyChecked && !player.isRidingMonster()) {
      QuestManager.qm.addCountToQuest("AvoidFallingStar");
      player.showEffect("Whew");
    }
  }

  public override void destroyByMonster() {
    QuestManager.qm.addCountToQuest("DestroyFallingStar");
  }

  override protected void afterEncounter() {
    QuestManager.qm.addCountToQuest("DestroyFallingStar");
  }

  public void nearPlayer(bool enter = true) {
    avoiding = enter;

    if (!enter && !alreadyChecked) alreadyChecked = true;
  }

  public bool isAlreadyChecked() {
    return alreadyChecked;
  }

  override public bool dangerous() {
    if (player.isAfterStrengthen() || player.isRidingMonster() || player.isUnstoppable() || player.isUsingRainbow()) return false;
    else return true;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }
}
