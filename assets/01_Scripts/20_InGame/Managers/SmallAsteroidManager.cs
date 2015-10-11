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

    if (objPrefab.transform.childCount > 0) {
      smallAsteroidsPrefab = new GameObject[objPrefab.transform.childCount];
      int count = 0;
      foreach (Transform tr in objPrefab.transform) {
        smallAsteroidsPrefab[count++] = tr.gameObject;

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
      smallAsteroidsPrefab = new GameObject[1];
      smallAsteroidsPrefab[0] = objPrefab;
      objPrefab.GetComponent<ObjectsMover>().setBoundingSize(objPrefab.GetComponent<Renderer>().bounds.extents.magnitude);
    }

    spawnManager.spawnRandom(smallAsteroidsPrefab, max_obstacles);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void respawn() {
    int count = max_obstacles - GameObject.FindGameObjectsWithTag("Obstacle_small").Length;
    if (count > 0) {
      spawnManager.spawnRandom(smallAsteroidsPrefab, count);
    }
  }
}
