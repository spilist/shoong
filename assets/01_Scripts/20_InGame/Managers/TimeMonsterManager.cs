using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeMonsterManager : ObjectsManager {
  public int spawnRadius = 250;
  public float speedScale = 1.5f;
  private bool spawned = false;

  override protected void spawn() {
    if (player == null) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  public bool isSpawned() {
    return spawned;
  }

  public void spawnMonster() {
    this.enabled = true;
    spawn();
    spawned = true;
  }

  public void stopMonster() {
    if (!spawned) return;

    spawned = false;
    instance.SetActive(false);
  }
}
