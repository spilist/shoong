using UnityEngine;
using System.Collections;

public class EnergizerMover : ObjectsMover {

  override public string getManager() {
    return "EnergizerManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  override public bool dangerous() {
    return false;
  }

  override protected void afterCollidePlayer() {
    destroyObject();
  }
}
