using UnityEngine;
using System.Collections;

public class SpecialPartsManager : ObjectsManager {
  public GameObject special;
  public float tumble = 3;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;

  override public void run() {
    StartCoroutine("spawnSpecial");
  }

  override public float getTumble(string objTag) {
    return tumble;
  }

  IEnumerator spawnSpecial() {
    float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
    yield return new WaitForSeconds(interval);

    spawnManager.spawn(special);
  }
}
