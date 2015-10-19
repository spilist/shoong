using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  private NormalPartsManager npm;

  protected override void initializeRest() {
    npm = (NormalPartsManager)objectsManager;
  }

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

    npm.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) npm.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
  }

  override public string getManager() {
    return "NormalPartsManager";
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}
