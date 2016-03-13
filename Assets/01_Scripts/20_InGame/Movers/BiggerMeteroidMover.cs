using UnityEngine;
using System.Collections;

public class BiggerMeteroidMover : ObjectsMover {
  override public string getManager() {
    return "BiggerMeteroidManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  override protected void afterCollidePlayer() {
    destroyObject();
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }
}
