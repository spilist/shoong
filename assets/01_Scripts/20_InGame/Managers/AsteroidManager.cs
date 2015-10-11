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

    if (objPrefab.transform.childCount > 0) {
      asteroidsPrefab = new GameObject[objPrefab.transform.childCount];
      int count = 0;
      foreach (Transform tr in objPrefab.transform) {
        asteroidsPrefab[count++] = tr.gameObject;

        float radius;
        if (tr.GetComponent<Renderer>() != null) {
          radius = tr.GetComponent<Renderer>().bounds.extents.magnitude;
        } else {
          Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
          foreach (Transform tra in tr) {
            bounds.Encapsulate(tra.GetComponent<Renderer>().bounds);
          }
          radius = bounds.extents.magnitude;
        }
        tr.GetComponent<ObjectsMover>().setBoundingSize(radius);
      }
    } else {
      asteroidsPrefab = new GameObject[1];
      asteroidsPrefab[0] = objPrefab;
      objPrefab.GetComponent<ObjectsMover>().setBoundingSize(objPrefab.GetComponent<Renderer>().bounds.extents.magnitude);
    }

    spawnManager.spawnRandom(asteroidsPrefab, max_obstacles);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void respawn() {
    int count = max_obstacles - GameObject.FindGameObjectsWithTag("Obstacle_big").Length;
    if (count > 0) {
      spawnManager.spawnRandom(asteroidsPrefab, count);
    }
  }
}
