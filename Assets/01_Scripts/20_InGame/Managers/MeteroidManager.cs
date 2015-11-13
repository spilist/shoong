using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MeteroidManager : ObjectsManager {
  public Transform meshes;

  public float warnPlayerDuring = 1;
  public float spawnRadius = 400;
  public int scatterAmount = 30;
  public int lineDistance = 1000;
  public GameObject fallingStarWarningLinePrefab;
  public List<GameObject> warningPool;
  public GameObject fallingStarSoundWarningPrefab;
  public List<GameObject> soundWarningPool;

  public float shortenRespawnPer = 10;
  public float shortenRespawnAmount = 0.1f;
  public int addSpeedAmount = 3;

  private Vector3 obstacleDirection;
  private Vector3 destination;

  override public void initRest() {
    warningPool = new List<GameObject>();
    soundWarningPool = new List<GameObject>();

    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(fallingStarWarningLinePrefab);
      obj.SetActive(false);
      warningPool.Add(obj);

      obj = (GameObject) Instantiate(fallingStarSoundWarningPrefab);
      obj.SetActive(false);
      soundWarningPool.Add(obj);
    }

    destroyWhenCollideSelf = true;

    StartCoroutine("spawnObstacle");
  }

  public void startSecond() {
    StartCoroutine("spawnObstacle");
  }

  override public void run() {}

  override public void runImmediately() {}

  GameObject getWarningLine() {
    return getPooledObj(warningPool, fallingStarWarningLinePrefab);
  }

  GameObject getObstacle(Vector3 pos) {
    return getPooledObj(objPool, objPrefab, pos);
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
}
