using UnityEngine;
using System.Collections;

public class PlayerDopple : MonoBehaviour {
  public float duration = 0.5f;
  private Color color;
  private float targetAlpha;
  private float alpha = 0;
  private Renderer mRenderer;
  private bool startFade = false;

	public void run(Mesh mesh, Material mat) {
    mRenderer = GetComponent<Renderer>();
    GetComponent<MeshFilter>().sharedMesh = mesh;
    mRenderer.material = mat;

    color = mat.color;
    targetAlpha = color.a / 2;
    alpha = 0;
    color.a = 0;
    mRenderer.material.color = color;

    startFade = true;
  }

  void Update () {
    if (startFade) {
      alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime * targetAlpha / duration);
      color.a = alpha;
      mRenderer.material.color = color;
      if (alpha == targetAlpha) Destroy(gameObject);
    }
	}
}
