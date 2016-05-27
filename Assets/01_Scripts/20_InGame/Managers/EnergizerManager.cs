using UnityEngine;
using System.Collections;

public class EnergizerManager : ObjectsManager {
  public float spawnRadius = 200;
  private Vector3 energizerDirection;
  public float playerSpeedIncreaseTo = 1.5f;
  public float speedRestoreStartAfter = 4;
  public float speedRestoreDuration = 1;

  override public void initRest() {
    destroyWhenCollideSelf = true;
    run();
  }
}
