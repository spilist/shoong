using UnityEngine;
using System.Collections;

public class DangerousEMPManager : ObjectsManager {
  public GameObject particleDestroyByPlayer;
  public int max_emp = 3;

  public float minScale = 15;
  public float maxScale = 25;
  public float startDuration = 1;
  public float decreaseDurationPerPulse = 0.1f;
  public float empScale = 70;
  public int empRotatingSpeed = 500;
  public float enlargeDuration = 0.1f;
  public float stayDuration = 1;
  public float shrinkDuration = 0.1f;
  private bool larger = false;
  public float enlargeScale = 2;

  override public void initRest() {
    spawnManager.spawn(objPrefab);
    ElapsedTime.time.startSpawnDangerousEMP();
  }

  override public void run() {}

  override public void runImmediately() {}

  public void respawn() {
    int count = max_emp - GameObject.FindGameObjectsWithTag("DangerousEMP").Length;
    if (count > 0) {
      GameObject obj = (GameObject) spawnManager.spawn(objPrefab);
      if (larger) obj.transform.localScale *= enlargeScale;
    }
  }

  public void startLarger() {
    larger = true;
    empScale *= enlargeScale;
  }
}
