using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowManyPartsGet : MonoBehaviour {
  private Text text;
  private Color color;
  private Vector2 position;
  private float disappearStartPos;
  private bool show = false;

  public float disappearLength = 18;

  void Update() {
    if (show) {
      color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
      text.color = color;

      position.y = Mathf.MoveTowards(position.y, disappearStartPos + disappearLength, Time.deltaTime * disappearLength);
      GetComponent<RectTransform>().anchoredPosition = position;
      if (color.a == 0) Destroy(gameObject);
    }
  }

  public void run(int partsGet) {
    text = GetComponent<Text>();
    color = text.color;
    position = GetComponent<RectTransform>().anchoredPosition;
    disappearStartPos = position.y;
    show = true;
    text.text = "+" + partsGet.ToString();
  }
}
