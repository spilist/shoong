using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  override protected void afterEncounter() {
    if (isMagnetized) QuestManager.qm.addCountToQuest("Blackhole");
    if (player.isUsingRainbow()) QuestManager.qm.addCountToQuest("GetPartsOnRainbow");
    if (player.isNearAsteroid()) {
      QuestManager.qm.addCountToQuest("GetPartsNearAsteroid");
      player.showEffect("Wow");
    }

    if (player.isUsingJetpack()) QuestManager.qm.addCountToQuest("GetPartsOnJetpack");

    objectsManager.run();
  }

  override public string getManager() {
    return "NormalPartsManager";
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}
