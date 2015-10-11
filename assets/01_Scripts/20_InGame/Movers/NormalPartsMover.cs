using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  override protected void afterEncounter() {
    if (isMagnetized) {
      DataManager.dm.increment("NumPartsAbsorbedWithBlackhole");
    }

    if (player.isUsingRainbow()) {
      DataManager.dm.increment("NumPartsGetOnRainbow");
    }

    if (player.isNearAsteroid()) {
      player.showEffect("Wow");
    }
  }

  override public string getManager() {
    return "NormalPartsManager";
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}
