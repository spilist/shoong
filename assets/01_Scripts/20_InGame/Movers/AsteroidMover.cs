using UnityEngine;
using System.Collections;

public class AsteroidMover : ObjectsMover {
  private bool isNearPlayer = false;
  private SpecialPartsManager spm;
  private AsteroidManager asm;
  private bool collideChecked = false;

  override public string getManager() {
    return "AsteroidManager";
  }

  override protected void initializeRest() {
    canBeMagnetized = false;
    asm = (AsteroidManager)objectsManager;
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();
  }

  override protected void afterDestroy() {
    if (isNearPlayer) player.nearAsteroid(false);
  }

  override public void destroyByMonster() {
    QuestManager.qm.addCountToQuest("DestroyAsteroid");
    QuestManager.qm.addCountToQuest("Monster");
  }

  override protected void afterEncounter() {
    QuestManager.qm.addCountToQuest("DestroyAsteroid");
    if (player.isUnstoppable()) {
      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd");
      QuestManager.qm.addCountToQuest("SpecialParts");
    } else if (player.isRidingMonster()) {
      QuestManager.qm.addCountToQuest("Monster");
    }

    if (isNearPlayer) player.nearAsteroid(false);

    objectsManager.run();
  }

  public void nearPlayer(bool enter = true) {
    if (collideChecked == enter) return;
    else collideChecked = enter;

    isNearPlayer = enter;

    player.nearAsteroid(enter);
  }

  override public bool dangerous() {
    if (player.isAfterStrengthen() || player.isRidingMonster() || player.isUnstoppable() || player.isUsingRainbow()) return false;
    else return true;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }
}
