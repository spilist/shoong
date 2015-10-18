using UnityEngine;
using System.Collections;

public class BlackholeManager : ObjectsManager {
  public int gravity = 50;
  public int gravityToUser = 130;
  public int gravityScale = 10;

  public int reboundDuring = 2;
  public float shakeAmount = 3;
  public int max_blackhole = 2;

  override public void initRest() {
    spawnManager.spawn(objPrefab);
    ElapsedTime.time.startBlackhole();
  }

  override public void run() {}

  override public void runImmediately() {}

  public Vector3 headingToBlackhole(Transform tr) {
    Vector3 heading = instance.transform.position - tr.position;
    return heading / heading.magnitude;
  }

  public void respawn() {
    int count = max_blackhole - GameObject.FindGameObjectsWithTag("Blackhole").Length;
    if (count > 0) {
      spawnManager.spawn(objPrefab);
    }
  }
}
