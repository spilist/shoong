using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FailedRhythm : MonoBehaviour {
  private Text text;
  private Color color;
  private Vector2 position;
  private float disappearStartPosX;
  private float disappearStartPosY;
  private bool show = false;

  private float stayCount = 0;

  public float stayDuring = 0;
  public float disappearDuring = 1;
  public float baseDisappearLengthY = 100;
  public float baseDisappearLengthX = 100;
  private float disappearLengthY;
  private float disappearLengthX;

  private float directionVariable = 1;

  private float originalX;
  private float originalY;
  private Color originalColor;

  void Awake() {
    text = GetComponent<Text>();
    originalX = GetComponent<RectTransform>().anchoredPosition.x;
    originalY = GetComponent<RectTransform>().anchoredPosition.y;
    originalColor = text.color;
  }

  void OnEnable() {
    color = originalColor;
    disappearStartPosX = originalX;
    disappearStartPosY = originalY;

    disappearLengthX = Random.Range(0, baseDisappearLengthX);
    disappearLengthY = Random.Range(baseDisappearLengthY * 0.8f, baseDisappearLengthY * 1.2f);

    if (Random.Range(0, 100) < 50) {
      directionVariable = -1;
    }
    show = true;
  }

  void Update() {
    if (show) {
      if (stayCount < stayDuring) {
        stayCount += Time.deltaTime;
      } else {
        color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / disappearDuring);
        text.color = color;
      }

      position.x = Mathf.MoveTowards(position.x, disappearStartPosX + disappearLengthX * directionVariable, Time.deltaTime * disappearLengthX * Random.Range(0.5f, 1.5f));
      position.y = Mathf.MoveTowards(position.y, disappearStartPosY + disappearLengthY, Time.deltaTime * disappearLengthY);
      GetComponent<RectTransform>().anchoredPosition = position;
      if (color.a == 0) {
        show = false;
        gameObject.SetActive(false);
      }
    }
  }

  void OnDisable() {
    GetComponent<RectTransform>().anchoredPosition = new Vector2(originalX, originalY);
    position = new Vector2(originalX, originalY);
    stayCount = 0;
    text.color = originalColor;
  }
}
