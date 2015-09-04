using UnityEngine;
using System.Collections;

public class BigObstaclesMover : ObjectsMover {
  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  protected override float strength() {
    return ((BasicObjectsManager)objectsManager).strength_obstacle;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);
  }

  public override void encounterPlayer() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    QuestManager.qm.addCountToQuest("DestroyAsteroid");
    QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd");
    Destroy(gameObject);
  }

  override public string getManager() {
    return "BasicObjectsManager";
  }
}
