using UnityEngine;
using System.Collections;

public class BlackMonsterManager : ObjectsManager {
  public float slowStayDuration = 1;
  public float increaseSpeedDuration = 2;
  public int increaseSpeedUntil = 180;
  public int detectDistance = 200;
  public int spawnRadius = 250;
  public float offScreenSpeedScale = 0.5f;
  public float blindDuration = 3f;
  public GameObject blindFilter;
  public float firstSpawnDelay = 2;

  override public void initRest() {
    Invoke("spawn", firstSpawnDelay);
  }

  override protected void spawn() {
    if (player == null || ScoreManager.sm.isGameOver()) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  override protected float spawnInterval() {
    return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  public void blindPlayer() {
    blindFilter.SetActive(true);
    Invoke("unblindPlayer", blindDuration);
  }

  void unblindPlayer() {
    blindFilter.SetActive(false);
  }
}
