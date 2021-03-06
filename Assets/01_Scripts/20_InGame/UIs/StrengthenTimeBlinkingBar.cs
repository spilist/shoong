﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrengthenTimeBlinkingBar : MonoBehaviour {
  public float blinkingSeconds = 0.2f;

  private Image image;
  private int[] angles;
  private int count;

	void Start () {
    image = GetComponent<Image>();
    angles = new int[] {0, -60, -90, -150, -180, -240, -270, -330};
	}

  public void timeElapse() {
    count--;
    transform.localRotation = Quaternion.Euler(0, 0, angles[count]);
  }

  public void startStrengthen(int val) {
    StopCoroutine("startBlink");
    image.enabled = true;
    count = val - 1;
    transform.localRotation = Quaternion.Euler(0, 0, angles[count]);
    StartCoroutine("startBlink");
  }

  public void stopStrengthen() {
    image.enabled = false;
    StopCoroutine("startBlink");
  }

  IEnumerator startBlink() {
    while (true) {
      yield return new WaitForSeconds(blinkingSeconds);
      image.enabled = !image.enabled;
    }
  }
}
