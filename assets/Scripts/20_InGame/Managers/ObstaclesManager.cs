using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObstaclesManager : MonoBehaviour {
  public GameObject[] obstacles;
  public float strength = 1;
  public float speed = 60;
  public float tumble = 1;
  public float minSpawnInterval = 0.5f;
  public float maxSpawnInterval = 5;
  public float warnPlayerDuring = 1;
  public float spawnRadius = 400;

  public int scatterAmount = 30;
  public int lineDistance = 1000;
  public GameObject fallingStarWarningLinePrefab;
  public GameObject fallingStarSoundWarningPrefab;

  public GameObject obsIndicatorPrefab;
  public float shortenRespawnPer = 10;
  public float shortenRespawnAmount = 0.1f;
  public int addSpeedAmount = 3;

  private Transform playerTransform;
  private Vector3 obstacleDirection;
  private Vector3 destination;

  void Start () {
    StartCoroutine("spawnObstacle");
    playerTransform = GameObject.Find("Player").transform;
  }

  IEnumerator spawnObstacle() {
    while(true) {
      GameObject prefab = obstacles[Random.Range(0, obstacles.Length)];
      yield return new WaitForSeconds(getInterval());

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;

      Vector3 spawnPos = screenToWorld(screenPos);
      obstacleDirection = playerPosScattered() - spawnPos;
      obstacleDirection.Normalize();
      destination = spawnPos + obstacleDirection * lineDistance;

      GameObject warningLine = (GameObject) Instantiate (fallingStarWarningLinePrefab);
      warningLine.GetComponent<FallingstarWarningLine>().run(spawnPos - 100 * obstacleDirection, destination, lineDistance + 100, warnPlayerDuring);

      Instantiate (fallingStarSoundWarningPrefab);

      // GameObject obsIndicator = (GameObject) Instantiate (obsIndicatorPrefab);
      // obsIndicator.transform.SetParent(UICanvas.transform, false);
      // obsIndicator.GetComponent<ObstacleIndicator>().run(spawnPos, warnPlayerDuring);

      yield return new WaitForSeconds(warnPlayerDuring);

      warningLine.GetComponent<FallingstarWarningLine>().erase();
      GameObject obstacle = (GameObject) Instantiate(prefab, spawnPos, Quaternion.identity);
      obstacle.transform.parent = gameObject.transform;
    }
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + playerTransform.position.x, playerTransform.position.y, screenPos.y + playerTransform.position.z);
  }

  Vector3 playerPosScattered() {
    return new Vector3(playerTransform.position.x + Random.Range(-scatterAmount, scatterAmount), playerTransform.position.y, playerTransform.position.z + Random.Range(-scatterAmount, scatterAmount));
  }

  public Vector3 getDirection() {
    return obstacleDirection;
  }

  float getInterval() {
    int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);

    return Random.Range(Mathf.Max(0, minSpawnInterval - timeUnit * shortenRespawnAmount), Mathf.Max(0, maxSpawnInterval - timeUnit * shortenRespawnAmount));
  }

  public float getSpeed() {
    int timeUnit = (int) Mathf.Floor(ElapsedTime.time.now / shortenRespawnPer);
    return (speed + timeUnit * addSpeedAmount);
  }
}
