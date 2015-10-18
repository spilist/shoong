using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
  }

  override public string getManager() {
    return "SpecialPartsManager";
  }
}
