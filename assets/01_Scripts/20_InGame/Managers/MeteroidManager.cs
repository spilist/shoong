using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MeteroidManager : ObjectsManager {
  public Transform meshes;
  public GameObject biggerMeteroid;
  public List<GameObject> biggerPool;

  public float biggerMeteroidStrength = 2.5f;
  public float biggerMeteroidSpeed = 400;
  public float biggerMeteroidTumble = 20;

  public float warnPlayerDuring = 1;
  public float spawnRadius = 400;
  public int scatterAmount = 30;
  public int lineDistance = 1000;
  public GameObject fallingStarWarningLinePrefab;
  public List<GameObject> warningPool;
  public GameObject biggerFallingStarWarningLinePrefab;
  public List<GameObject> biggerWarningPool;
  public GameObject fallingStarSoundWarningPrefab;
  public List<GameObject> soundWarningPool;

  public float shortenRespawnPer = 10;
  public float shortenRespawnAmount = 0.1f;
  public int addSpeedAmount = 3;

  private Vector3 obstacleDirection;
  private Vector3 destination;
  private bool phaseStarted = false;

  override public void initRest() {
    biggerPool = new List<GameObject>();
    warningPool = new List<GameObject>();
    biggerWarningPool = new List<GameObject>();
    soundWarningPool = new List<GameObject>();

    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(biggerMeteroid);
      obj.SetActive(false);
      obj.transform.parent = transform;
      biggerPool.Add(obj);

      obj = (GameObject) Instantiate(fallingStarWarningLinePrefab);
      obj.SetActive(false);
      warningPool.Add(obj);

      obj = (GameObject) Instantiate(biggerFallingStarWarningLinePrefab);
      obj.SetActive(false);
      biggerWarningPool.Add(obj);

      obj = (GameObject) Instantiate(fallingStarSoundWarningPrefab);
      obj.SetActive(false);
      soundWarningPool.Add(obj);
    }

    StartCoroutine("spawnObstacle");
  }

  public void startPhase() {
    phaseStarted = true;
  }

  public void startSecond() {
    StartCoroutine("spawnObstacle");
  }

  override public void run() {}

  override public void runImmediately() {}

  GameObject getWarningLine() {
    if (phaseStarted) return getPooledObj(biggerWarningPool, biggerFallingStarWarningLinePrefab);
    else return getPooledObj(warningPool, fallingStarWarningLinePrefab);
  }

  GameObject getObstacle(Vector3 pos) {
    if (phaseStarted) return getPooledObj(biggerPool, biggerMeteroid, pos);
    else return getPooledObj(objPool, objPrefab, pos);
  }

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

      GameObject warningLine = getWarningLine();
      warningLine.SetActive(true);
      warningLine.GetComponent<FallingstarWarningLine>().run(spawnPos - 100 * obstacleDirection, destination, lineDistance + 100, warnPlayerDuring);

      getPooledObj(soundWarningPool, fallingStarSoundWarningPrefab).SetActive(true);

      yield return new WaitForSeconds(warnPlayerDuring);

      warningLine.GetComponent<FallingstarWarningLine>().erase();

      GameObject obstacle = getObstacle(spawnPos);
      obstacle.SetActive(true);
      obstacle.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
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

  public Mesh getRandomMesh() {
    return meshes.GetChild(Random.Range(0, meshes.childCount)).GetComponent<MeshFilter>().sharedMesh;
  }

  // override protected float spawnInterval() {
  //   int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);

  //   return Random.Range(Mathf.Max(0, minSpawnInterval - timeUnit * shortenRespawnAmount), Mathf.Max(0, maxSpawnInterval - timeUnit * shortenRespawnAmount));
  // }

  // override public float getSpeed() {
  //   int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);
  //   return (speed + timeUnit * addSpeedAmount);
  // }
}
