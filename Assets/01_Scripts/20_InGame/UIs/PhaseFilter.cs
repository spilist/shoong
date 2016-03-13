using UnityEngine;
using System.Collections;

public class PhaseFilter : MonoBehaviour {
	// private ProceduralMaterial mat;
  private Material mat;
  public float fluctuatingAlpha = 0.04f;
  public float weakAlpha = 0.08f;
  public float strongAlpha = 0.16f;
  public float alphaChangingDuration = 1;
  public Color blue;
  public Color purple;
  public Color red;
  Color color;
  float alpha = 0;
  int alphaStatus = 0;

  void OnEnable() {
    // mat = GetComponent<Renderer>().material as ProceduralMaterial;
    // mat.SetProceduralFloat("$randomseed", Random.Range(0, 1000));
    // mat.RebuildTextures();
    mat = GetComponent<Renderer>().material;
    color = mat.GetColor("_Color");
    nextPhase(2);
  }

  public void nextPhase(int num) {
    if (num == 2) {
      alphaStatus = 1;
      mat.SetColor("_Emission", blue);
    } else if (num == 3) {
      alphaStatus = 4;
    } else if (num == 4) {
      mat.SetColor("_Emission", purple);
    } else if (num == 5) {
      mat.SetColor("_Emission", red);
    }
  }

  void Update() {
    if (alphaStatus == 1) {
      alpha = Mathf.MoveTowards(alpha, weakAlpha - fluctuatingAlpha, fluctuatingAlpha * Time.deltaTime);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == weakAlpha - fluctuatingAlpha) {
        alphaStatus = 2;
      }
    } else if (alphaStatus == 2) {
      alpha = Mathf.MoveTowards(alpha, weakAlpha + fluctuatingAlpha, Time.deltaTime * fluctuatingAlpha / alphaChangingDuration);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == weakAlpha + fluctuatingAlpha) {
        alphaStatus = 3;
      }
    } else if (alphaStatus == 3) {
      alpha = Mathf.MoveTowards(alpha, weakAlpha - fluctuatingAlpha, Time.deltaTime * fluctuatingAlpha / alphaChangingDuration);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == weakAlpha - fluctuatingAlpha) {
        alphaStatus = 2;
      }
    } else if (alphaStatus == 4) {
      alpha = Mathf.MoveTowards(alpha, strongAlpha - fluctuatingAlpha, fluctuatingAlpha * Time.deltaTime);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == strongAlpha - fluctuatingAlpha) {
        alphaStatus = 5;
      }
    } else if (alphaStatus == 5) {
      alpha = Mathf.MoveTowards(alpha, strongAlpha + fluctuatingAlpha, Time.deltaTime * fluctuatingAlpha / alphaChangingDuration);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == strongAlpha + fluctuatingAlpha) {
        alphaStatus = 6;
      }
    } else if (alphaStatus == 6) {
      alpha = Mathf.MoveTowards(alpha, strongAlpha - fluctuatingAlpha, Time.deltaTime * fluctuatingAlpha / alphaChangingDuration);
      color.a = alpha;
      mat.SetColor("_Color", color);
      if (alpha == strongAlpha - fluctuatingAlpha) {
        alphaStatus = 5;
      }
    }
  }
}
