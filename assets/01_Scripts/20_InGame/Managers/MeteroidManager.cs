﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeteroidManager : ObjectsManager {
  private GameObject[] meteroidsPrefab;
  public GameObject biggerMeteroids;
  private GameObject[] biggerMeteroidsPrefab;
  public float biggerMeteroidStrength = 2.5f;
  public float biggerMeteroidSpeed = 400;
  public float biggerMeteroidTumble = 20;

  public float warnPlayerDuring = 1;
  public float spawnRadius = 400;
  public int scatterAmount = 30;
  public int lineDistance = 1000;
  public GameObject fallingStarWarningLinePrefab;
  public GameObject biggerFallingStarWarningLinePrefab;
  public GameObject fallingStarSoundWarningPrefab;

  public float shortenRespawnPer = 10;
  public float shortenRespawnAmount = 0.1f;
  public int addSpeedAmount = 3;

  private Vector3 obstacleDirection;
  private Vector3 destination;
  private bool phaseStarted = false;

  override public void initRest() {
    isNegative = true;

    meteroidsPrefab = new GameObject[objPrefab.transform.childCount];
    int count = 0;
    foreach (Transform tr in objPrefab.transform) {
      meteroidsPrefab[count++] = tr.gameObject;
    }

    biggerMeteroidsPrefab = new GameObject[biggerMeteroids.transform.childCount];
    count = 0;
    foreach (Transform tr in biggerMeteroids.transform) {
      biggerMeteroidsPrefab[count++] = tr.gameObject;
    }

    StartCoroutine("spawnObstacle");
  }

  public void startPhase() {
    phaseStarted = true;
  }

  override public void run() {}

  override public void runImmediately() {}

  IEnumerator spawnObstacle() {
    while(true) {
      yield return new WaitForSeconds(spawnInterval());

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;

      Vector3 spawnPos = screenToWorld(screenPos);
      obstacleDirection = playerPosScattered() - spawnPos;
      obstacleDirection.Normalize();
      destination = spawnPos + obstacleDirection * lineDistance;

      bool normal = true;
      if (phaseStarted) {
        normal = Random.Range(0, 100) < 50;
      }

      GameObject warningLine;
      if (normal) warningLine = (GameObject) Instantiate (fallingStarWarningLinePrefab);
      else warningLine = (GameObject) Instantiate (biggerFallingStarWarningLinePrefab);

      warningLine.GetComponent<FallingstarWarningLine>().run(spawnPos - 100 * obstacleDirection, destination, lineDistance + 100, warnPlayerDuring);

      Instantiate (fallingStarSoundWarningPrefab);

      yield return new WaitForSeconds(warnPlayerDuring);

      warningLine.GetComponent<FallingstarWarningLine>().erase();

      GameObject obstacle;
      if (normal) obstacle = (GameObject) Instantiate(meteroidsPrefab[Random.Range(0, meteroidsPrefab.Length)], spawnPos, Quaternion.identity);
      else obstacle = (GameObject) Instantiate(biggerMeteroidsPrefab[Random.Range(0, meteroidsPrefab.Length)], spawnPos, Quaternion.identity);

      obstacle.transform.parent = transform;
    }
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
  }

  Vector3 playerPosScattered() {
    return player.transform.position + player.getDirection() * player.getSpeed() + new Vector3(Random.Range(-scatterAmount, scatterAmount), 0, Random.Range(-scatterAmount, scatterAmount));
  }

  override public Vector3 getDirection() {
    return obstacleDirection;
  }

  override protected float spawnInterval() {
    int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);

    return Random.Range(Mathf.Max(0, minSpawnInterval - timeUnit * shortenRespawnAmount), Mathf.Max(0, maxSpawnInterval - timeUnit * shortenRespawnAmount));
  }

  override public float getSpeed() {
    int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);
    return (speed + timeUnit * addSpeedAmount);
  }
}
