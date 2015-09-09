using UnityEngine;
using System.Collections;

public class SpecialPartsManager : ObjectsManager {
  public float[] destroyBonusPerLevel;

  public GameObject special;
  public float tumble = 3;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;
  public float bonus;

  override public void initRest() {
    bonus = destroyBonusPerLevel[DataManager.dm.getInt("SpecialPartsLevel") - 1];
  }

  override public void run() {
    foreach (GameObject specialPart in GameObject.FindGameObjectsWithTag("SpecialPart")) {
      Destroy(specialPart);
    }
    StartCoroutine("spawnSpecial");
  }

  override public float getTumble(string objTag) {
    return tumble;
  }

  IEnumerator spawnSpecial() {
    if (!skipInterval) {
      float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
      yield return new WaitForSeconds(interval);
    }

    skipInterval = false;

    spawnManager.spawn(special);
  }

  override public void skipRespawnInterval() {
    skipInterval = true;
  }
}
