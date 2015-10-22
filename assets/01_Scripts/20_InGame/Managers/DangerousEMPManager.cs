using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DangerousEMPManager : ObjectsManager {
  public GameObject particleDestroyByPlayer;
  public List<GameObject> particleDestroyByPlayerPool;

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
    ElapsedTime.time.startSpawnDangerousEMP();

    particleDestroyByPlayerPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(particleDestroyByPlayer);
      obj.SetActive(false);
      particleDestroyByPlayerPool.Add(obj);
    }

    respawn();
  }

  override public void run() {}

  override public void runImmediately() {}

  override public void respawn() {
    int count = objAmount - GameObject.FindGameObjectsWithTag("DangerousEMP").Length;
    if (count > 0) {
      GameObject obj = getPooledObj(objPool, objPrefab, spawnManager.getSpawnPosition(objPrefab));
      if (larger) obj.transform.localScale = enlargeScale * Vector3.one;
    }
  }

  public void startLarger() {
    larger = true;
    empScale *= enlargeScale;
  }
}
