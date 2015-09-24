using UnityEngine;
using System.Collections;

public class AsteroidManager : ObjectsManager {
  public GameObject brokenAsteroidsPrefab;
  public int brokenTumble = 20;
  public int minBrokenSpawn = 3;
  public int maxBrokenSpawn = 5;
  public float minBrokenSize = 0.5f;
  public float maxBrokenSize = 1.5f;
  public int minBrokenSpeed = 400;
  public int maxBrokenSpeed = 1200;

  public int max_obstacles = 3;
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
