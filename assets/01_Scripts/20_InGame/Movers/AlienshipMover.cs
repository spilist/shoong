using UnityEngine;
using System.Collections;

public class AlienshipMover : ObjectsMover {
  private AlienshipManager asm;

  protected override void initializeRest() {
    asm = (AlienshipManager)objectsManager;
    canBeMagnetized = false;
  }

  protected override void normalMovement() {
    direction = getDirection();
    rb.velocity = direction * getSpeed();
    rb.angularVelocity = Vector3.zero;
  }

  override public string getManager() {
    return "AlienshipManager";
  }

  override public bool dangerous() {
    return true;
  }

  override protected float getSpeed() {
    // change needed
    return objectsManager.getSpeed();
  }

  override protected float getTumble() {
    return 0;
  }
}
