using UnityEngine;
using System.Collections;

public class NormalPartsManager : ObjectsManager {
  public int max_parts = 60;
  private GameObject[] partsPrefab;

  override public void initRest() {
    partsPrefab = new GameObject[objPrefab.transform.childCount];
    int count = 0;
    foreach (Transform tr in objPrefab.transform) {
      partsPrefab[count++] = tr.gameObject;
    }
    spawnManager.spawnRandom(partsPrefab, max_parts);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void respawn() {
    int count = max_parts - GameObject.FindGameObjectsWithTag("Part").Length;
    if (count > 0) {
      spawnManager.spawnRandom(partsPrefab, count);
    }
  }
}
