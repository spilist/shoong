using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweetBoxManager : ObjectsManager {
  public float generateAfter = 0.4f;
  public float generatingDuration = 1.1f;
  public int numGeneration = 50;
  public float spawnRadius = 200;

	override public void initRest() {
    run();
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
}
