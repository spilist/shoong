using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
  }

  override public string getManager() {
    return "SpecialPartsManager";
  }
}
