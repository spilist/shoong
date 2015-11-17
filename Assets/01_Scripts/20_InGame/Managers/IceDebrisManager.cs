using UnityEngine;
using System.Collections;

public class IceDebrisManager : ObjectsManager {
  public float spawnRadius = 200;
  private Vector3 iceDirection;
  public float playerSpeedReduceTo = 0.2f;
  public float speedRestoreDuring = 3;

  override public void initRest() {
    destroyWhenCollideSelf = true;

    StartCoroutine("spawnIce");
  }

  override public void run() {}

  override public void runImmediately() {}

  IEnumerator spawnIce() {
    while(true) {
      yield return new WaitForSeconds(spawnInterval());

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;

      Vector3 spawnPos = screenToWorld(screenPos);
      iceDirection = playerPos() - spawnPos;
      iceDirection.Normalize();

      GameObject ice = getPooledObj(objPool, objPrefab, spawnPos);
      ice.SetActive(true);
    }
  }

  public void stopSpawn() {
    StopCoroutine("spawnIce");
  }

  Vector3 playerPos() {
    return player.transform.position + player.getDirection() * player.getSpeed();
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
  }

  override public Vector3 getDirection() {
    return iceDirection;
  }
}
