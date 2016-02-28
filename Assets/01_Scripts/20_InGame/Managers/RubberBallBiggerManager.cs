using UnityEngine;
using System.Collections;

public class RubberBallBiggerManager : ObjectsManager {
  private int origObjAmount;
  public int objAmountBigger = 12;

  override public void initRest() {
    origObjAmount = objAmount;
    spawnPooledObjs(objPool, objPrefab, objAmount);
    TimeManager.time.startRubberBallBigger();
  }

  override public void run() {}

  override public void runImmediately() {}

  public void setMany(bool many) {
    if (many) {
      objAmount = objAmountBigger;
    } else {
      objAmount = origObjAmount;
    }
  }
}
