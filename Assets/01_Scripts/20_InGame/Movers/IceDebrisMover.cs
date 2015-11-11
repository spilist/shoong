using UnityEngine;
using System.Collections;

public class IceDebrisMover : ObjectsMover {

  override public string getManager() {
    return "IceDebrisManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterCollidePlayer() {
    destroyObject();
  }
}
