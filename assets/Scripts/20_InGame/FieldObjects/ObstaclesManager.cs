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
  public Canvas UICanvas;

  public GameObject obsIndicatorPrefab;

  private float screenWidth;
  private float screenHeight;

	void Start () {
    StartCoroutine("spawnObstacle");
	}

	void Update () {
	}

  IEnumerator spawnObstacle() {
    while(true) {
      float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
      yield return new WaitForSeconds(interval);

      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      GameObject obsIndicator = (GameObject) Instantiate (obsIndicatorPrefab);
      obsIndicator.transform.SetParent(UICanvas.transform, false);
      obsIndicator.GetComponent<ObstacleIndicator>().run(screenPos, warnPlayerDuring);
      yield return new WaitForSeconds(warnPlayerDuring);

      GameObject prefab = obstacles[Random.Range(0, obstacles.Length)];
      Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.y));
      GameObject obstacle = (GameObject) Instantiate(prefab, spawnPos, Quaternion.identity);
      obstacle.transform.parent = gameObject.transform;
    }
  }
}
