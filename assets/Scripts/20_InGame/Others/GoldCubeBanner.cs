﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldCubeBanner : MonoBehaviour {
  public float moveTo;
  public float moveDuration = 0.5f;
  public float showDuration = 2;
  public int offset = 30;

  private float moveBackTo;
  private Text cubes;
  private int count;
  private RectTransform tr;
  private float positionX;
  private float showCount = 0;
  private int moveStatus = 0;

	void OnEnable() {
    cubes = GetComponent<Text>();
    count = (int)GameController.control.goldenCubes["now"];
    cubes.text = count.ToString();

    tr = GetComponent<RectTransform>();
    positionX = cubes.preferredWidth + offset;
    tr.anchoredPosition += new Vector2(positionX, 0);
  }

  public void add(int amount = 1) {
    count += amount;
    cubes.text = count.ToString();

    GameController.control.goldenCubes["now"] = count;
    GameController.control.goldenCubes["total"] = ((int)GameController.control.goldenCubes["total"]) + amount;

    moveBackTo = cubes.preferredWidth + offset;
    showCount = 0;
    moveStatus = 1;
  }

  void Update() {
    if (moveStatus == 1) {
      positionX = Mathf.MoveTowards(positionX, moveTo, Time.deltaTime / moveDuration * (moveBackTo - moveTo));
      tr.anchoredPosition = new Vector2(positionX, tr.anchoredPosition.y);
      if (positionX == moveTo) moveStatus = 2;
    } else if (moveStatus == 2) {
      if (showCount < showDuration) {
        showCount += Time.deltaTime;
      } else {
        moveStatus = 3;
      }
    } else if (moveStatus == 3) {
      positionX = Mathf.MoveTowards(positionX, moveBackTo, Time.deltaTime / moveDuration * (moveBackTo - moveTo));
      tr.anchoredPosition = new Vector2(positionX, tr.anchoredPosition.y);
      if (positionX == moveBackTo) moveStatus = 4;
    }
  }
}
