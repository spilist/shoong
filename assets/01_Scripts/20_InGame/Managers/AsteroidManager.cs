using UnityEngine;
using System.Collections;

public class AsteroidManager : ObjectsManager {
  public int brokenTumble = 30;
  public int minBrokenSpawn = 6;
  public int maxBrokenSpawn = 8;
  public float minBrokenSize = 0.5f;
  public float maxBrokenSize = 1f;
  public int minBrokenSpeed = 200;
  public int maxBrokenSpeed = 400;

  public float minSizeAfterBreak = 5;
  public float maxSizeAfterBreak = 10;
  public float destroyLargeAfter = 0.5f;
  public float destroySmallAfter = 4;

  public int max_obstacles = 4;
  private GameObject[] asteroidsPrefab;

  override public void initRest() {
    isNegative = true;

    asteroidsPrefab = new GameObject[objPrefab.transform.childCount];
    int count = 0;
    foreach (Transform tr in objPrefab.transform) {
      asteroidsPrefab[count++] = tr.gameObject;
    }

    spawnManager.spawnRandom(asteroidsPrefab, max_obstacles);
  }

  override public void run() {
    if (GameObject.FindGameObjectsWithTag("Obstacle_big").Length < max_obstacles) {
      spawnManager.spawn(asteroidsPrefab[Random.Range(0, asteroidsPrefab.Length)]);
    }
  }

  override public void runImmediately() {
    run();
  }
}
