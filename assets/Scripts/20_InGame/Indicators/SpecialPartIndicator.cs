using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialPartIndicator : MonoBehaviour {
  public float detectDistance = 1;

  private bool isIndicating = false;
  private float screenWidth;
  private float screenHeight;

  public float widthOffset = 30;
  public float heightOffset = 50;

	void Start () {
    GetComponent<Image>().enabled = false;
    screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    screenHeight = transform.parent.GetComponent<RectTransform>().rect.height;
	}

	void Update () {
    if (isIndicating) {
      showIndicate();
    }
	}

  void showIndicate() {
    GameObject target = GameObject.FindWithTag("Monster");
    if (target == null) return;

    Vector2 targetPos = Camera.main.WorldToViewportPoint(target.transform.position);

    if (targetPos.x < -detectDistance || targetPos.x > (detectDistance + 1) || targetPos.y < -detectDistance || targetPos.y > (detectDistance + 1)) {
      GetComponent<Image>().enabled = false;
      return;
    } else if (targetPos.x >= 0.0f && targetPos.x <= 1.0f && targetPos.y >= 0.0f && targetPos.y <= 1.0f) {
      GetComponent<Image>().enabled = false;
      return;
    }
    else {
      GetComponent<Image>().enabled = true;
      // Player와의 각도를 얻음
      Vector2 direction = new Vector2(targetPos.x - 0.5f, targetPos.y - 0.5f);
      float angle = Mathf.Atan2 (direction.x, direction.y);
      transform.localEulerAngles = new Vector3(0, 0, -angle * Mathf.Rad2Deg);

      direction.x = Mathf.Sin (angle);
      direction.y = Mathf.Cos (angle);

      GetComponent<RectTransform>().anchoredPosition = new Vector2(direction.x * (screenWidth - widthOffset) / 2, direction.y * (screenHeight - heightOffset) / 2);
    }
  }

  public void startIndicate() {
    isIndicating = true;
  }

  public void stopIndicate() {
    isIndicating = false;
    GetComponent<Image>().enabled = false;
  }
}
