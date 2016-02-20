using UnityEngine;
using System.Collections;

public class ConfusedMonsterManager : ObjectsManager {
	public float slowStayDuration = 1;
  public float increaseSpeedDuration = 1.2f;
  public int increaseSpeedUntil = 240;
  public int detectDistance = 200;
  public int spawnRadius = 250;
  public float offScreenSpeedScale = 0.5f;
  public GameObject confusedEffect;
  public float confusedDuration = 3f;

  override public void initRest() {
    spawn();
  }

  override protected void spawn() {
    if (player == null || ScoreManager.sm.isGameOver()) return;
    Debug.Log("confused??2");

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

  public void confusePlayer() {
    Player.pl.setConfused(true);
    confusedEffect.SetActive(true);
    Invoke("unconfusePlayer", confusedDuration);
  }

  void unconfusePlayer() {
    confusedEffect.SetActive(false);
    Player.pl.setConfused(false);
  }
}
