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

  override public void run() {
    if (GameObject.FindGameObjectsWithTag("Part").Length < max_parts) {
      spawnManager.spawn(partsPrefab[Random.Range(0, partsPrefab.Length)]);
    }
  }

  override public void runImmediately() {
    run();
  }
}
