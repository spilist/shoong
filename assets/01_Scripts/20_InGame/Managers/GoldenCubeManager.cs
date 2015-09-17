using UnityEngine;
using System.Collections;

public class GoldenCubeManager : ObjectsManager {
  public GoldCubesCount gcCount;

  public int respawnAfter = 120;

  public float spawnRadius = 200;
  public float detectDistance = 200;

  public float generateCubePer = 0.3f;
  public GameObject energyCubePrefab;

  private bool firstSpawn = true;

  override public void initRest() {
    run();
  }

  override protected void spawn() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);

    instance = (GameObject) Instantiate(objPrefab, spawnPos, Quaternion.identity);
    instance.transform.parent = transform;
  }

  override protected float spawnInterval() {
    if (firstSpawn) {
      firstSpawn = false;
      return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
    else return respawnAfter;
  }
}
