using UnityEngine;
using System.Collections;

public class MetalMover : ObjectsMover {
  override protected void initializeRest() {
  }

  override public string getManager() {
    return "MetalManager";
  }

  override public int cubesWhenDestroy() {
    return 50;
  }
}
