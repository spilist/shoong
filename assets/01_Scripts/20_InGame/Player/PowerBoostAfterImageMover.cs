﻿using UnityEngine;
using System.Collections;

public class PowerBoostAfterImageMover : MonoBehaviour {
  float appearAfter;
  float duration;
  Renderer mRenderer;
  Color color;
  float originalAlpha;
  float alpha;
  bool startFade = false;

  public void run(float appearAfter, float duration, Color mainColor, Color emissiveColor, float scale) {
    this.duration = duration;
    this.appearAfter = appearAfter;

    mRenderer = GetComponent<Renderer>();
    mRenderer.material.color = mainColor;
    mRenderer.material.SetColor("_Emission", emissiveColor);

    color = mainColor;
    alpha = color.a;
    originalAlpha = alpha;
    transform.localScale = scale * Vector3.one;
    StartCoroutine("appear");
  }

  IEnumerator appear() {
    yield return new WaitForSeconds(appearAfter);
    mRenderer.enabled = true;
    startFade = true;
  }

	void Update () {
    if (startFade) {

      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime * originalAlpha / duration);
      color.a = alpha;
      mRenderer.material.color = color;
      if (alpha == 0) Destroy(gameObject);
    }
	}
}