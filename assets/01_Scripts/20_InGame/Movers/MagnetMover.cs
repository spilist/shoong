using UnityEngine;
using System.Collections;

public class MagnetMover : ObjectsMover {
  private MagnetManager magnetManager;
  public int power;

  override protected void initializeRest() {
    magnetManager = (MagnetManager)objectsManager;
    canBeMagnetized = false;

    power = magnetManager.power;
  }

  override public string getManager() {
    return "MagnetManager";
  }

  // override protected void afterDestroy(bool byPlayer) {
  //   player.magnetizeEnd();
  // }

  // override protected void afterEncounter() {
  //   player.magnetizeEnd();
  // }
}
