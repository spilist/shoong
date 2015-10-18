using UnityEngine;
using System.Collections;

public class TransformerMover : ObjectsMover {
  override protected void initializeRest() {
  }

  override public string getManager() {
    return "TransformerManager";
  }

  override public int cubesWhenDestroy() {
    return 50;
  }
}
