using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowEnergyChange : MonoBehaviour {
  private Text text;
  private Color color;
  private Vector2 position;
  private float disappearStartPos;
  private bool show = false;

  public float disappearLength = 18;
  public int sign;
  public string changeDirection;

	void Update () {
    if (show) {
      color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
      text.color = color;

      position.y = Mathf.MoveTowards(position.y, disappearStartPos + disappearLength * sign, Time.deltaTime * disappearLength);
      GetComponent<RectTransform>().anchoredPosition = position;
      if (color.a == 0) Destroy(gameObject);
    }
	}

  public void run(int amount) {
    text = GetComponent<Text>();
    color = text.color;
    position = GetComponent<RectTransform>().anchoredPosition;
    disappearStartPos = position.y;
    show = true;
    text.text = changeDirection + (Mathf.Abs(amount)).ToString();
  }
}
