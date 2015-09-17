using UnityEngine;
using System.Collections;

public class AsteroidManager : ObjectsManager {
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
