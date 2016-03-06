using UnityEngine;
using System.Collections;

public class RubberBallManager : ObjectsManager {
  override public void initRest() {
    spawnPooledObjs(objPool, objPrefab, objAmount);
    // TimeManager.time.startRubberBall();
  }

  override public void run() {}

  override public void runImmediately() {}
}
