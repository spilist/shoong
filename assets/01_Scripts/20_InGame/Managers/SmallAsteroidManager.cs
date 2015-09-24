using UnityEngine;
using System.Collections;

public class SmallAsteroidManager : ObjectsManager {
	public int max_obstacles = 3;
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
