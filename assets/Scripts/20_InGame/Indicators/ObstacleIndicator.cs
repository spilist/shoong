using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObstacleIndicator : MonoBehaviour {
  private bool isIndicating = false;
  private GameObject[] obstacles;
  private float screenWidth;
  private float screenHeight;

  public float widthOffset = 30;
  public float heightOffset = 50;

	void Start () {
    gameObject.SetActive(false);
    screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;
	}

	void Update () {
    if (isIndicating) {
      showIndicate();
    }
	}

  void showIndicate() {
    obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

    foreach (GameObject obstacle in obstacles) {
      Vector2 targetPos = Camera.main.WorldToViewportPoint(obstacle.transform.position);

      if (targetPos.x >= 0.0f && targetPos.x <= 1.0f && targetPos.y >= 0.0f && targetPos.y <= 1.0f) {
        continue;
      }
      else {
        gameObject.SetActive(true);
        // Player와의 각도를 얻음
        Vector2 direction = new Vector2(targetPos.x - 0.5f, targetPos.y - 0.5f);
        float angle = Mathf.Atan2 (direction.x, direction.y);
        transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);

        direction.x = Mathf.Sin (angle);
        direction.y = Mathf.Cos (angle);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(direction.x * (screenWidth - widthOffset) / 2, direction.y * (screenHeight - heightOffset) / 2);
      }
    }
  }

  public void startIndicate() {
    isIndicating = true;
    gameObject.SetActive(true);
  }

  public void stopIndicate() {
    isIndicating = false;
    gameObject.SetActive(false);
  }
}
