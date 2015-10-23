using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoldenCubeManager : ObjectsManager {
  public GoldCubesCount gcCount;

  public int respawnAfter = 120;

  public float spawnRadius = 200;
  public float detectDistance = 200;

  public float generateCubePer = 0.3f;
  public GameObject energyCubePrefab;
  public int cubeAmount = 30;
  public List<GameObject> cubePool;

  private bool firstSpawn = true;

  override public void initRest() {
    cubePool = new List<GameObject>();
    for (int i = 0; i < cubeAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(energyCubePrefab);
      obj.SetActive(false);
      cubePool.Add(obj);
    }

    run();
  }

  override protected void spawn() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);

    instance = getPooledObj(objPool, objPrefab, spawnPos);
  }

  override protected float spawnInterval() {
    if (firstSpawn) {
      firstSpawn = false;
      return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
    else return respawnAfter;
  }

  public void goldenDestroyEffect(Vector3 pos) {
    GameObject obj = getPooledObj(objDestroyEffectPool, objDestroyEffect, pos);
    obj.SetActive(true);
  }
}
