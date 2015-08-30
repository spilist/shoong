using UnityEngine;
using System.Collections;

public class BasicObjectsManager : ObjectsManager {
  public GameObject[] parts;
  public int max_parts = 50;
  public GameObject partsDestroy;

  public GameObject[] obstacles_big;
  public int max_obstacles = 5;

  public float speed_parts = 0;
  public float speed_obstacles = 15;

  public float tumble_parts = 1;
  public float tumble_obstacles = 0.5f;

  override public void run() {
    spawnManager.spawnRandom(parts, max_parts);
    spawnManager.spawnRandom(obstacles_big, max_obstacles);
  }

  void Update() {
    if (GameObject.FindGameObjectsWithTag("Part").Length < max_parts) {
      spawnManager.spawnRandom(parts, 1);
    }

    if (GameObject.FindGameObjectsWithTag("Obstacle_big").Length < max_obstacles) {
      spawnManager.spawnRandom(obstacles_big, 1);
    }
  }

  override public float getSpeed(string objTag) {
    float speed = 0;

    if (objTag == "Part") speed = speed_parts;
    else if (objTag == "Obstacle_big") speed = speed_obstacles;

    return speed;
  }

  override public float getTumble(string objTag) {
    float tumble = 0;

    if (objTag == "Part") tumble = tumble_parts;
    else if (objTag == "Obstacle_big") tumble = tumble_obstacles;

    return tumble;
  }
}
