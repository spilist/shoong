using UnityEngine;
using System.Collections;

public class VacuumAlienshipManager : ObjectsManager {
  public int spawnRadius = 250;
  public int detectDistance = 200;
  public int headFollowingSpeed = 100;
  public float offScreenSpeedScale = 0.5f;

  public int gravity = 150;
  public int gravityToCandies = 150;
  public float gravityScale = 10;
  public float firstSpawnDelay = 2;

	override public void initRest() {
    // player = Player.pl;
    Invoke("spawn", firstSpawnDelay);
  }

  override protected void spawn() {
    if (player == null || ScoreManager.sm.isGameOver()) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.transform.rotation = Quaternion.LookRotation(player.transform.position - spawnPos);
    instance.SetActive(true);
  }

  override protected float spawnInterval() {
    return Random.Range(minSpawnInterval, maxSpawnInterval);
  }
}
