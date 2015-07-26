using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObstaclesManager : MonoBehaviour {
  public GameObject[] obstacles;
  public float speed = 60;
  public float tumble = 1;
  public float minSpawnInterval = 0.5f;
  public float maxSpawnInterval = 5;
  public float warnPlayerDuring = 1;
  public float spawnRadius = 400;
  public Canvas UICanvas;

  public GameObject obsIndicatorPrefab;

  private Transform playerTransform;

  void Start () {
    StartCoroutine("spawnObstacle");
    playerTransform = GameObject.Find("Player").transform;
  }

  void Update () {
  }

  IEnumerator spawnObstacle() {
    while(true) {
      GameObject prefab = obstacles[Random.Range(0, obstacles.Length)];
      float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
      yield return new WaitForSeconds(interval);

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;
      GameObject obsIndicator = (GameObject) Instantiate (obsIndicatorPrefab);
      obsIndicator.transform.SetParent(UICanvas.transform, false);
      obsIndicator.GetComponent<ObstacleIndicator>().run(screenPos, warnPlayerDuring);
      yield return new WaitForSeconds(warnPlayerDuring);

      Vector3 spawnPos = new Vector3(screenPos.x + playerTransform.position.x, playerTransform.position.y, screenPos.y + playerTransform.position.z);

      GameObject obstacle = (GameObject) Instantiate(prefab, spawnPos, Quaternion.identity);
      obstacle.transform.parent = gameObject.transform;
    }
  }
}
