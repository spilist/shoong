using UnityEngine;
using System.Collections;

public class PhaseMonsterManager : ObjectsManager {
  public float slowStayDuration = 1;
  public float increaseSpeedDuration = 2;
  public int increaseSpeedUntil = 140;
  // public float chargeDuration = 0.5f;
  // public float rushDuration = 1;
  // public float rushSpeed = 400;
  public int detectDistance = 200;
  public int spawnRadius = 200;
  public float offScreenSpeedScale = 0.8f;

  override public void initRest() {
    for (int i = 0; i < objAmount; i++) spawn();
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
