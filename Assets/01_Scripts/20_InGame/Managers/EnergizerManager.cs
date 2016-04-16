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

    StartCoroutine("spawnEnergizer");
  }

  override public void run() {}

  override public void runImmediately() {}

  IEnumerator spawnEnergizer() {
    while(true) {
      yield return new WaitForSeconds(spawnInterval());

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;

      Vector3 spawnPos = screenToWorld(screenPos);
      energizerDirection = playerPos() - spawnPos;
      energizerDirection.Normalize();

      GameObject energizer = getPooledObj(objPool, objPrefab, spawnPos);
      energizer.SetActive(true);
    }
  }

  public void stopSpawn() {
    StopCoroutine("spawnEnergizer");
  }

  Vector3 playerPos() {
    return player.transform.position + player.getDirection() * player.getSpeed();
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
  }

  override public Vector3 getDirection() {
    return energizerDirection;
  }
}
