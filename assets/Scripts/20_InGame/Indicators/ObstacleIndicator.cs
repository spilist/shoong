using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObstacleIndicator : MonoBehaviour {
  private float screenWidth;
  private float screenHeight;
  private bool isWarning = false;

  public float widthOffset = 30;
  public float heightOffset = 50;

  private Vector2 spawnPosition;
  private float warnPlayerDuring;
  private Image image;

	void Start () {
    screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;
    image = GetComponent<Image>();
    image.enabled = false;
	}

	void Update () {
    if (isWarning && warnPlayerDuring > 0) {
      image.enabled = true;
      warnPlayerDuring -= Time.deltaTime;
      showWarning();
    } else {
      Destroy(gameObject);
    }
	}

  public void run(Vector2 pos, float during) {
    isWarning = true;
    warnPlayerDuring = during;
    Vector3 playerPosition = GameObject.Find("Player").transform.position;
    Vector3 spawnWorld = new Vector3(pos.x + playerPosition.x, playerPosition.y, pos.y + playerPosition.z);
    spawnPosition = Camera.main.WorldToViewportPoint(spawnWorld);
  }

  void showWarning() {
    // Player와의 각도를 얻음
    Vector2 direction = new Vector2(spawnPosition.x - 0.5f, spawnPosition.y - 0.5f);
    float angle = Mathf.Atan2 (direction.x, direction.y);
    transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);

    direction.x = Mathf.Sin (angle);
    direction.y = Mathf.Cos (angle);

    GetComponent<RectTransform>().anchoredPosition = new Vector2(direction.x * (screenWidth - widthOffset) / 2, direction.y * (screenHeight - heightOffset) / 2);
  }
}
