﻿using UnityEngine;
using System.Collections;

public class UIEffect : MonoBehaviour {
  public float startScale = 0.1f;
  public float stayScale = 1;
  public float smallScale = 0.6f;
  public float largeScale = 1.2f;
  private float origLargeScale;
  private float origStayScale;

  public float changeTime = 0.1f;
  public float stayTime = 2f;
  public bool stayBeforeUse = false;
  public float shrinkDuration = 4f;
  public float shrinkScale = 0.5f;

  private float scale;
  private int status = 0;
  private float stayCount = 0;

  private bool shrinking = false;

	void Awake() {
    origLargeScale = largeScale;
    origStayScale = stayScale;
  }

  void OnEnable() {
    // foreach (Transform tr in transform.parent) {
    //   if (tr.gameObject != gameObject) tr.gameObject.SetActive(false);
    // }

    transform.localScale = startScale * Vector3.one;
    scale = startScale;
    largeScale = origLargeScale;
    stayScale = origStayScale;
    status++;
  }

  void Update() {
    if (status > 0) {
      if (status == 1) {
        changeScale(largeScale, largeScale - startScale);
      } else if (status == 2) {
        changeScale(smallScale, smallScale - largeScale);
      } else if (status == 3) {
        changeScale(stayScale, stayScale - smallScale);
      } else if (status == 4) {
        if (stayCount < stayTime) stayCount += Time.deltaTime;
        else if (stayBeforeUse) {
          status = 8;
          shrinking = true;
        }
        else status++;
      } else if (status == 5) {
        changeScale(largeScale, largeScale - stayScale);
      } else if (status == 6) {
        changeScale(startScale, startScale - largeScale);
      } else if (status == 7) {
        gameObject.SetActive(false);
      }

      if (shrinking) {
        scale = Mathf.MoveTowards(scale, shrinkScale, Time.deltaTime * (stayScale - shrinkScale) / shrinkDuration);
      }

      transform.localScale = scale * Vector3.one;
    }
  }

  public void diminish() {
    shrinking = false;
    largeScale = origLargeScale * scale;
    stayScale = origStayScale * scale;
    status = 5;
  }

  void OnDisable() {
    status = 0;
    stayCount = 0;
  }

  void changeScale(float targetScale, float difference) {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (scale == targetScale) status++;
  }
}
