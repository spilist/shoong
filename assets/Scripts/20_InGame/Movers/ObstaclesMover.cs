using UnityEngine;
using System.Collections;

public class ObstaclesMover : ObjectsMover {
  ObstaclesManager obm;
  SpecialPartsManager spm;
  private bool avoiding = false;
  private bool alreadyChecked = false;

  protected override void initializeRest() {
    obm = GameObject.Find("Field Objects").GetComponent<ObstaclesManager>();
    spm = obm.GetComponent<SpecialPartsManager>();

    canBeMagnetized = false;
  }

  protected override float getSpeed() {
    return obm.getSpeed();
  }

  protected override float getTumble() {
    return obm.tumble;
  }

  protected override Vector3 getDirection() {
    return obm.getDirection();
  }

  protected override float strength() {
    return obm.strength;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(player.obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);

    if (avoiding && !alreadyChecked) {
      QuestManager.qm.addCountToQuest("AvoidFallingStar");
      player.showEffect("Whew");
    }
  }

  public override void destroyByMonster() {
    QuestManager.qm.addCountToQuest("DestroyFallingStar");
  }

  public override void encounterPlayer() {
    Instantiate(player.obstacleDestroy, transform.position, transform.rotation);
    QuestManager.qm.addCountToQuest("DestroyFallingStar");

    Destroy(gameObject);
  }

  override protected void doSomethingSpecial(Collision collision) {
    if (QuestManager.qm.doingQuest("FallingStarReboundByCubeDispenser") && collision.collider.gameObject.tag == "CubeDispenser") {
      Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
      if (pos.x >= 0.0f && pos.x <= 1.0f && pos.y >= 0.0f && pos.y <= 1.0f) {
        QuestManager.qm.addCountToQuest("FallingStarReboundByCubeDispenser");
      }
    }
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

  override public int cubesWhenEncounter() {
    return obm.cubesWhenEncounter;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (obm.cubesWhenEncounter * spm.bonus) : 0;
  }

  override public bool isNegativeObject() {
    return true;
  }
}
