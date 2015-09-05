using UnityEngine;
using System.Collections;

public class BigObstaclesMover : ObjectsMover {
  public int width;
  private PlayerMover playerMover;

  protected override void initializeRest() {
    canBeMagnetized = false;
    playerMover = player.GetComponent<PlayerMover>();
  }

  protected override float strength() {
    return ((BasicObjectsManager)objectsManager).strength_obstacle;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(playerMover.obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);
  }

  public override void encounterPlayer() {
    Instantiate(playerMover.obstacleDestroy, transform.position, transform.rotation);
    QuestManager.qm.addCountToQuest("DestroyAsteroid");

    if (playerMover.isUnstoppable()) {
      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd");
      QuestManager.qm.addCountToQuest("SpecialParts");
    } else if (playerMover.isRidingMonster()) {
      QuestManager.qm.addCountToQuest("Monster");
    } else if (playerMover.isUsingRainbow()) {
      QuestManager.qm.addCountToQuest("RainbowDonuts");
    }

    Destroy(gameObject);
  }

  override public string getManager() {
    return "BasicObjectsManager";
  }
}
