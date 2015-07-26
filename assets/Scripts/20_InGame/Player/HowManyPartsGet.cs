using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowManyPartsGet : MonoBehaviour {
  private ComboBar comboBar;

  private Text text;
  private Color color;
  private Vector2 position;
  private float disappearStartPos;

  public float disappearLength = 18;

	void Start () {
    comboBar = GameObject.Find("Bars Canvas").GetComponent<ComboBar>();

    text = GetComponent<Text>();
    text.text = "+" + comboBar.getComboRatio().ToString();

    color = text.color;
    position = GetComponent<RectTransform>().anchoredPosition;
    disappearStartPos = position.y;
  }

  void Update() {
    color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
    text.color = color;

    position.y = Mathf.MoveTowards(position.y, disappearStartPos + disappearLength, Time.deltaTime * disappearLength);
    GetComponent<RectTransform>().anchoredPosition = position;

    if (color.a == 0) Destroy(gameObject);
  }
}
