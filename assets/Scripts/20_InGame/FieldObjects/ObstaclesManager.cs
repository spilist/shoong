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
  public float obstacleCircleScale = 1.2f;
  public Canvas UICanvas;

  public GameObject obsIndicatorPrefab;

  void Start () {
    StartCoroutine("spawnObstacle");
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
      GameObject obsIndicator = (GameObject) Instantiate (obsIndicatorPrefab);
      obsIndicator.transform.SetParent(UICanvas.transform, false);
      obsIndicator.GetComponent<ObstacleIndicator>().run(screenPos, warnPlayerDuring);
      yield return new WaitForSeconds(warnPlayerDuring);

      screenPos.x = (screenPos.x - 0.5f) * obstacleCircleScale + 0.5f;
      screenPos.y = (screenPos.y - 0.5f) * obstacleCircleScale + 0.5f;

      Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y));
      GameObject obstacle = (GameObject) Instantiate(prefab, spawnPos, Quaternion.identity);
      obstacle.transform.parent = gameObject.transform;
    }
  }
}
