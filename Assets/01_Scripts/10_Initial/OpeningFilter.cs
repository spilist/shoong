using UnityEngine;
using System.Collections;

public class OpeningFilter : MonoBehaviour {
  public GameObject firstCandy;
  public GameObject dreamingText;
  public float fadeIn = 2;
  private float alpha;
  private float targetAlpha;
  private Color color;
  private Renderer mRenderer;

  void Start() {
  }

	void Update () {

	}

  public IEnumerator startOpening() {
    alpha = 1;
    TrackingManager.tm.firstPlayLog("1_OpeningScene");
    yield return opening();
  }

  public IEnumerator opening() {
    yield return goAlpha(0);

    firstCandy.SetActive(true);
    dreamingText.SetActive(true);
    gameObject.SetActive(false);
  }

  public IEnumerator goAlpha(float newAlpha) {
    mRenderer = GetComponent<MeshRenderer>();
    color = mRenderer.sharedMaterial.color;
    targetAlpha = newAlpha;
    while (true) {
      alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime / fadeIn);
      color.a = alpha;
      mRenderer.material.color = color;
      yield return null;
      if (alpha == targetAlpha) {
        break;
      }
    }

  }
}
