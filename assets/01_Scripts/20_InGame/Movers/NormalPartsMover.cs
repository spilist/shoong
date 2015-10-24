using UnityEngine;
using System.Collections;

public class NormalPartsMover : ObjectsMover {
  private NormalPartsManager npm;
  private MeshFilter filter;

  protected override void initializeRest() {
    npm = (NormalPartsManager)objectsManager;
    filter = GetComponent<MeshFilter>();
  }

  protected override void afterEnable() {
    filter.sharedMesh = npm.getRandomMesh();
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

    // npm.ptb.checkCollected(filter.sharedMesh);
  }

  // override protected void afterDestroy(bool byPlayer) {
  //   if (byPlayer) npm.ptb.checkCollected(filter.sharedMesh);
  // }

  override public string getManager() {
    return "NormalPartsManager";
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}
