using UnityEngine;
using System.Collections;

public class PhaseMonsterManager : ObjectsManager {
  public int startSpawnAmount = 2;
  public int speed_runaway = 60;
  public int detectDistance = 200;
  public int spawnRadius = 200;

  override public void initRest() {
    for (int i = 0; i < startSpawnAmount; i++) spawn();
  }

  override protected void spawn() {
    if (player == null) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  public void increaseSpawn() {
    spawn();
  }
}
