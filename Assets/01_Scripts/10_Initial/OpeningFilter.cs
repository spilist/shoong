using UnityEngine;
using System.Collections;

public class OpeningFilter : MonoBehaviour {
  public GameObject firstCandy;
  public GameObject dreamingText;
  public float fadeIn = 2;
  private float alpha;
  private Color color;
  private Renderer mRenderer;

  void Start() {
    mRenderer = GetComponent<MeshRenderer>();
    color = mRenderer.sharedMaterial.color;
    alpha = 1;
    FacebookManager.fb.firstPlayLog("1_OpeningScene");
  }

	void Update () {
    alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / fadeIn);
    color.a = alpha;
    mRenderer.material.color = color;
    if (alpha == 0) {
      firstCandy.SetActive(true);
      dreamingText.SetActive(true);
      gameObject.SetActive(false);
    }
	}
}
