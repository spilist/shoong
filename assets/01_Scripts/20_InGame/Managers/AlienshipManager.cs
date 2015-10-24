using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienshipManager : ObjectsManager {
  public GameObject laserPrefab;
  public GameObject laserWarningLinePrefab;
  public List<GameObject> laserPool;
  public List<GameObject> laserWarningPool;
  public int laserPoolAmount = 30;

  public int spawnRadius = 200;
  public int headFollowingSpeed = 100;
  public int speedIncreaseAmount = 10;
  public float speedIncreasePer = 5;
  public int shootLaserPer = 10;
  public float chargeTime = 0.5f;

  public int laserLoseEnergy = 70;
  public int laserRadius = 20;
  public int laserLength = 500;
  public float laserShootingDuration = 0.5f;
  public float laserStayDuration = 1;
  public float laserShrinkingDuration = 0.3f;
  public int laserRotatingSpeed = 1000;
  private int timeSpawned;

  override public void initRest() {
    laserPool = new List<GameObject>();
    laserWarningPool = new List<GameObject>();
    for (int i = 0; i < laserPoolAmount; ++i) {
      GameObject laser = (GameObject) Instantiate(laserPrefab);
      laser.SetActive(false);
      laserPool.Add(laser);

      GameObject laserWarning = (GameObject) Instantiate(laserWarningLinePrefab);
      laserWarning.SetActive(false);
      laserWarningPool.Add(laserWarning);
    }
    run();
  }

  public GameObject getLaser(Vector3 pos) {
    return getPooledObj(laserPool, laserPrefab, pos);
  }

  public GameObject getLaserWarning(Vector3 pos) {
    return getPooledObj(laserWarningPool, laserWarningLinePrefab, pos);
  }

  override public float getSpeed() {
    if (player.isUsingEMP()) {
      return 0;
    } else if (Vector3.Distance(player.transform.position, instance.transform.position) < 10) {
      return player.getSpeed();
    }
    else {
      int timeUnit = (int) Mathf.Floor((TimeManager.time.now - timeSpawned) / speedIncreasePer);
      return (speed + timeUnit * speedIncreaseAmount);
    }
  }

  override protected void spawn() {
    if (player == null) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.transform.rotation = Quaternion.LookRotation(player.transform.position - spawnPos);
    instance.SetActive(true);
  }

  override protected void afterSpawn() {
    timeSpawned = TimeManager.time.now;
  }
}
