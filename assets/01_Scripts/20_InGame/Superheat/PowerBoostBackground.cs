using UnityEngine;
using System.Collections;

public class PowerBoostBackground : MonoBehaviour {
  public float alphaChangeUpDuration = 0.1f;
  public float alphaChangeDownDuration = 0.5f;

  // private ProceduralMaterial mat;
  private Material mat;

  Color color;
  float alpha = 0;
  int fadeStatus = 0;

	void Start () {
    mat = GetComponent<Renderer>().material;
    // mat = GetComponent<Renderer>().material as ProceduralMaterial;
    color = mat.GetColor("_TintColor");
    color.a = 0;
    mat.SetColor("_TintColor", color);
  }

  public void startPowerBoost() {
    gameObject.SetActive(true);
    alpha = 0;
    fadeStatus = 1;
  }

  public void stopPowerBoost() {
    fadeStatus = 2;
  }

	void Update () {
    if (fadeStatus == 1) {
      alpha = Mathf.MoveTowards(alpha, 1, Time.deltaTime / alphaChangeUpDuration);
      color.a = alpha;
      mat.SetColor("_TintColor", color);
      if (alpha == 1) fadeStatus = 0;
    } else if (fadeStatus == 2) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / alphaChangeDownDuration);
      color.a = alpha;
      mat.SetColor("_TintColor", color);
      if (alpha == 0) {
        fadeStatus = 0;
        gameObject.SetActive(false);
      }
    }
	}

  void OnDisable() {
    // mat.SetProceduralFloat("$randomseed", Random.Range(0, 1000));
    // mat.RebuildTextures();
    fadeStatus = 0;
  }
}
