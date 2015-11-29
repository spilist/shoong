using UnityEngine;
using System.Collections;

public class PowerBoostAfterImageMover : MonoBehaviour {
  float duration;
  Renderer mRenderer;
  Color color;
  float originalAlpha;
  float alpha;
  bool startFade = false;

  public void run(float duration, Color color, float scale) {
  // public void run(float duration, Color mainColor, Color emissiveColor, float scale) {
    this.duration = duration;

    mRenderer = GetComponent<Renderer>();
    // mRenderer.material.color = mainColor;
    // mRenderer.material.SetColor("_Emission", emissiveColor);

    this.color = color;
    alpha = color.a;
    originalAlpha = alpha;
    transform.localScale = scale * Vector3.one;
    mRenderer.enabled = true;
    startFade = true;
  }

	void Update () {
    if (startFade) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime * originalAlpha / duration);
      color.a = alpha;
      mRenderer.material.color = color;
      if (alpha == 0) {
        startFade = false;
        gameObject.SetActive(false);
      }
    }
	}
}
