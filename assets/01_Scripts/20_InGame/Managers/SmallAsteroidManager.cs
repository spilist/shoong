using UnityEngine;
using System.Collections;

public class SmallAsteroidManager : ObjectsManager {
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

  public int max_obstacles = 6;
  private GameObject[] smallAsteroidsPrefab;

  override public void initRest() {
    isNegative = true;

    smallAsteroidsPrefab = new GameObject[objPrefab.transform.childCount];
    int count = 0;
    foreach (Transform tr in objPrefab.transform) {
      smallAsteroidsPrefab[count++] = tr.gameObject;
    }

    spawnManager.spawnRandom(smallAsteroidsPrefab, max_obstacles);
  }

  override public void run() {
    if (GameObject.FindGameObjectsWithTag("Obstacle_big").Length < max_obstacles) {
      spawnManager.spawn(smallAsteroidsPrefab[Random.Range(0, smallAsteroidsPrefab.Length)]);
    }
  }

  override public void runImmediately() {
    run();
  }
}
