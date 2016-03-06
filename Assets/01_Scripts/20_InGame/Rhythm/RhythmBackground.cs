using UnityEngine;
using System.Collections;
using SynchronizerData;

public class RhythmBackground : MonoBehaviour {
  public static RhythmBackground rb;
  
  public float currentAlpha;
  private Renderer mRenderer;
  private Color color;
  private float alpha;
  private Animation beatAnimation;
  // private Animation anim;

  void Awake() {
    rb = this;

    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
    mRenderer = GetComponent<MeshRenderer>();
    color = mRenderer.sharedMaterial.GetColor("_TintColor");
    alpha = color.a;
    // anim = GetComponent<Animation>();
  }

  void Update () {
    color.a = currentAlpha;
    mRenderer.material.SetColor("_TintColor", color);
	}
}
