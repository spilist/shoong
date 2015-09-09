using UnityEngine;
using System.Collections;

public class BigObstaclesMover : ObjectsMover {
  private bool isNearPlayer = false;
  private SpecialPartsManager spm;

  protected override void initializeRest() {
    canBeMagnetized = false;
    spm = objectsManager.GetComponent<SpecialPartsManager>();
  }

  protected override float strength() {
    return ((BasicObjectsManager)objectsManager).strength_obstacle;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(player.obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);

    if (isNearPlayer) player.nearAsteroid(false);
  }

  public override void encounterPlayer() {
    Instantiate(player.obstacleDestroy, transform.position, transform.rotation);
    QuestManager.qm.addCountToQuest("DestroyAsteroid");

    if (player.isUnstoppable()) {
      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd");
      QuestManager.qm.addCountToQuest("SpecialParts");
    } else if (player.isRidingMonster()) {
      QuestManager.qm.addCountToQuest("Monster");
    } else if (player.isUsingRainbow()) {
      QuestManager.qm.addCountToQuest("RainbowDonuts");
    }

    Destroy(gameObject);
  }

  override public string getManager() {
    return "BasicObjectsManager";
  }

  public void nearPlayer(bool enter = true) {
    isNearPlayer = enter;
  }

  override public bool dangerous() {
    if (player.isUnstoppable() || player.isUsingRainbow()) return false;
    else return true;
  }

  override public int cubesWhenEncounter() {
    int cubes = ((BasicObjectsManager)objectsManager).cubesByBigObstacle;
    return player.isUnstoppable()? (int) (cubes * spm.bonus) : cubes;
  }
}
