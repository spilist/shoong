using UnityEngine;
using System.Collections;

public class BigObstaclesMover : ObjectsMover {
  private bool isNearPlayer = false;
  private SpecialPartsManager spm;
  private int cubes;
  private bool collideChecked = false;

  protected override void initializeRest() {
    canBeMagnetized = false;
    spm = objectsManager.GetComponent<SpecialPartsManager>();
    cubes = ((BasicObjectsManager)objectsManager).cubesByBigObstacle;
  }

  protected override float strength() {
    return ((BasicObjectsManager)objectsManager).strength_obstacle;
  }

  public override void destroyObject(bool destroyEffect = true) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }

    if (destroyEffect) {
      Instantiate(player.obstacleDestroy, transform.position, transform.rotation);
    }

    if (isNearPlayer) player.nearAsteroid(false);

    Destroy(gameObject);
  }

  public override void encounterPlayer() {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }

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

    if (isNearPlayer) player.nearAsteroid(false);

    Destroy(gameObject);
  }

  override public string getManager() {
    return "BasicObjectsManager";
  }

  public void nearPlayer(bool enter = true) {
    if (collideChecked == enter) return;
    else collideChecked = enter;

    isNearPlayer = enter;

    player.nearAsteroid(enter);
  }

  override public bool dangerous() {
    if (player.isUnstoppable() || player.isUsingRainbow()) return false;
    else return true;
  }

  override public int cubesWhenEncounter() {
    return cubes;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubes * spm.bonus) : 0;
  }

  override public bool isNegativeObject() {
    return true;
  }
}
