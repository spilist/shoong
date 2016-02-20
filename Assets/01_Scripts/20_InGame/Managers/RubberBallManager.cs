using UnityEngine;
using System.Collections;

public class RubberBallManager : ObjectsManager {
  private int origObjAmount;
  public int objAmountBigger = 12;
  public float smallScale = 1.5f;
  public float bigScale = 2.5f;
  public float objScale;

  private bool bigger = false;

  override public void initRest() {
    origObjAmount = objAmount;
    objScale = smallScale;
    spawnPooledObjs(objPool, objPrefab, objAmount);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void setBigger(bool val) {
    bigger = val;
    if (val) {
      objAmount = objAmountBigger;
      objScale = bigScale;
    } else {
      objAmount = origObjAmount;
      objScale = smallScale;
    }
  }
}
