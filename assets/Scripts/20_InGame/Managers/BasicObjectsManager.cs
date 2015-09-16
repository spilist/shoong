using UnityEngine;
using System.Collections;

public class BasicObjectsManager : ObjectsManager {
  public Transform normalParts;
  public int max_parts = 50;
  public GameObject partsDestroy;

  public GameObject[] obstacles_big;
  public int max_obstacles = 5;

  public float speed_parts = 0;
  public float speed_obstacles = 15;

  public float tumble_parts = 1;
  public float tumble_obstacles = 0.5f;

  public float strength_obstacle = 2;
  public int cubesByBigObstacle = 15;

  private bool respawn = false;
  private GameObject[] partsPrefab;

  override public void run() {
    partsPrefab = new GameObject[normalParts.childCount];
    int count = 0;
    foreach (Transform tr in normalParts) {
      partsPrefab[count++] = tr.gameObject;
    }
    spawnManager.spawnRandom(partsPrefab, max_parts);
    spawnManager.spawnRandom(obstacles_big, max_obstacles);
    foreach (Transform tr in transform) {
      tr.gameObject.SetActive(false);
    }
  }

  void Update() {
    if (respawn) {
      if (GameObject.FindGameObjectsWithTag("Part").Length < max_parts) {
        spawnManager.spawnRandom(partsPrefab, 1);
      }

      if (GameObject.FindGameObjectsWithTag("Obstacle_big").Length < max_obstacles) {
        spawnManager.spawnRandom(obstacles_big, 1);
      }
    }
  }

  override public float getSpeed(string objTag) {
    float speed = 0;

    if (objTag == "Part" || objTag == "SummonedPart") speed = speed_parts;
    else if (objTag == "Obstacle_big") speed = speed_obstacles;

    return speed;
  }

  override public float getTumble(string objTag) {
    float tumble = 0;

    if (objTag == "Part" || objTag == "SummonedPart") tumble = tumble_parts;
    else if (objTag == "Obstacle_big") tumble = tumble_obstacles;

    return tumble;
  }

  public void startRespawn() {
    respawn = true;
  }
}
