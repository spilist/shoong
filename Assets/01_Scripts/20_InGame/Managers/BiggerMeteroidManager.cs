using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BiggerMeteroidManager : ObjectsManager {
  public Transform meshes;

  public float warnPlayerDuring = 0.5f;
  public float spawnRadius = 250;
  public int lineDistance = 600;
  public GameObject fallingStarWarningLinePrefab;
  public List<GameObject> warningPool;
  public GameObject fallingStarSoundWarningPrefab;
  public List<GameObject> soundWarningPool;
  public Material goldenMaterial;
  public Material originalMaterial;

  private Vector3 obstacleDirection;
  private Vector3 destination;

  override protected void beforeInit() {
    if (DataManager.dm.isBonusStage) {
      objPrefab.GetComponent<Renderer>().sharedMaterial = goldenMaterial;
    }
  }

  public void restoreMaterial() {
    objPrefab.GetComponent<Renderer>().sharedMaterial = originalMaterial;
  }

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

    if (DataManager.dm.isBonusStage) {
      Invoke("startSecond", 1);
    }
  }

  public void startSecond(bool on) {
    if (on) StartCoroutine("spawnObstacle2");
    else StopCoroutine("spawnObstacle2");
  }

  override public void run() {}

  override public void runImmediately() {}

  GameObject getWarningLine() {
    return getPooledObj(warningPool, fallingStarWarningLinePrefab);
  }

  GameObject getObstacle(Vector3 pos) {
    return getPooledObj(objPool, objPrefab, pos);
  }

  public void stopSpawn() {
    StopCoroutine("spawnObstacle");
    StopCoroutine("spawnObstacle2");
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
      warningLine.GetComponent<FallingstarWarningLine>().run(spawnPos - 100 * obstacleDirection, destination, lineDistance + 100, warnPlayerDuring);
      warningLine.SetActive(true);

      getPooledObj(soundWarningPool, fallingStarSoundWarningPrefab).SetActive(true);

      yield return new WaitForSeconds(warnPlayerDuring);

      warningLine.GetComponent<FallingstarWarningLine>().erase();

      GameObject obstacle = getObstacle(spawnPos);
      obstacle.SetActive(true);
      obstacle.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
    }
  }

  IEnumerator spawnObstacle2() {
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
      warningLine.GetComponent<FallingstarWarningLine>().run(spawnPos - 100 * obstacleDirection, destination, lineDistance + 100, warnPlayerDuring);
      warningLine.SetActive(true);

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
    return player.transform.position + player.getDirection() * player.getSpeed();
  }

  override public Vector3 getDirection() {
    return obstacleDirection;
  }

  public Mesh getRandomMesh() {
    return meshes.GetChild(Random.Range(0, meshes.childCount)).GetComponent<MeshFilter>().sharedMesh;
  }
}
