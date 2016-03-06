using UnityEngine;
using System.Collections;

public class PhaseFilter : MonoBehaviour {
	private ProceduralMaterial mat;
  public float startAlpha = 0.08f;
  public Color dangerousColor;
  Color color;

  void OnEnable() {
    mat = GetComponent<Renderer>().material as ProceduralMaterial;
    mat.SetProceduralFloat("$randomseed", Random.Range(0, 1000));
    mat.RebuildTextures();
    color = mat.GetColor("_TintColor");
    color.a = startAlpha;
  }

  public void nextPhase(int num) {
    if (num == 2) {
      mat.SetColor("_TintColor", color);
    } else if (num == 3) {
      color.a = startAlpha * 2;
      mat.SetColor("_TintColor", color);
    } else if (num == 4) {
      mat.SetProceduralColor("Liquid_Color", dangerousColor);
      mat.RebuildTextures();
    }
  }
}
