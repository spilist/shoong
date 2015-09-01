﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowChangeText : MonoBehaviour {
  private Text text;
  private Color color;
  private Color iconColor;
  private Vector2 position;
  private float disappearStartPosX;
  private float disappearStartPosY;
  private bool show = false;
  private Renderer icon;

  public int sign = 1;
  public string changeDirection;
  public bool hasIcon = true;

  public float disappearLengthY = 18;
  public float disappearLengthX = 10;
  private float directionVariable = 1;
  public float changeBase;
  public float changeScale;
  public float maxScale;

  void Update() {
    if (show) {
      color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
      text.color = color;
      if (hasIcon) {
        iconColor.a = color.a;
        icon.material.color = iconColor;
      }

      position.x = Mathf.MoveTowards(position.x, disappearStartPosX + disappearLengthX * directionVariable, Time.deltaTime * disappearLengthX * Random.Range(0.5f, 1.5f));
      position.y = Mathf.MoveTowards(position.y, disappearStartPosY + disappearLengthY * sign, Time.deltaTime * disappearLengthY * Random.Range(0.5f, 1.5f));
      GetComponent<RectTransform>().anchoredPosition = position;
      if (color.a == 0) Destroy(gameObject);
    }
  }

  public void run(int val) {
    int amount = Mathf.Abs(val);

    text = GetComponent<Text>();
    color = text.color;
    position = GetComponent<RectTransform>().anchoredPosition;
    disappearStartPosX = position.x;
    disappearStartPosY = position.y;
    show = true;
    text.text = changeDirection + amount.ToString();

    disappearLengthX = Random.Range(0, disappearLengthX);
    disappearLengthY = Random.Range(disappearLengthY * 0.8f, disappearLengthY * 1.2f);

    if (Random.Range(0, 100) < 50) {
      directionVariable = -1;
    }

    float changeAmount = ((amount / changeBase) - 1) * changeScale;
    changeAmount = Mathf.Min(changeAmount, changeScale * maxScale);
    text.fontSize += (int) changeAmount;

    if (hasIcon) {
      icon = transform.Find("Icon").GetComponent<Renderer>();
      iconColor = icon.material.color;

      float iconChangeAmount = ((amount / changeBase) - 1) * (changeScale - 1);
      iconChangeAmount = Mathf.Min(iconChangeAmount, (changeScale - 1) * maxScale);

      icon.transform.localScale += Vector3.one * iconChangeAmount;
      icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-icon.transform.localScale.x, 0, 0);
    }
  }
}
