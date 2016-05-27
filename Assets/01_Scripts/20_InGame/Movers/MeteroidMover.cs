using UnityEngine;
using System.Collections;

public class MeteroidMover : ObjectsMover {
  override public string getManager() {
    return "MeteroidManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  override protected void afterCollidePlayer(bool effect) {
    destroyObject();
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }
}
