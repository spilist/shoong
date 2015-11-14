using UnityEngine;
using System.Collections;
using SynchronizerData;

public class RhythmBackground : MonoBehaviour {
  public static RhythmBackground rb;

  public float minAlpha = 0.17f;
  public float maxAlpha = 0.55f;
  private Renderer mRenderer;
  private Color color;
  private float alpha;
  private bool alphaUp = false;
  private bool alphaDown = false;
  private BeatObserver beatObserver;
  private Animation anim;

	void Awake() {
    rb = this;

    beatObserver = GetComponent<BeatObserver>();
    mRenderer = GetComponent<MeshRenderer>();
    color = mRenderer.sharedMaterial.GetColor("_TintColor");
    alpha = color.a;
    anim = GetComponent<Animation>();
  }

  void Update () {
    if ((beatObserver.beatMask & BeatType.OffBeat) == BeatType.OffBeat) {
      alphaUp = true;
      alphaDown = false;
    //   anim.Play();
    }

    if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
      alphaUp = false;
      alphaDown = true;
    }

    if (alphaUp) {
      alpha = Mathf.MoveTowards(alpha, maxAlpha, Time.deltaTime * (maxAlpha - minAlpha) / (RhythmManager.rm.samplePeriod / 2));
      color.a = alpha;
      mRenderer.material.SetColor("_TintColor", color);
    }

    if (alphaDown) {
      alpha = Mathf.MoveTowards(alpha, minAlpha, Time.deltaTime * (maxAlpha - minAlpha) / (RhythmManager.rm.samplePeriod / 2));
      color.a = alpha;
      mRenderer.material.SetColor("_TintColor", color);
    }
	}

  public void goUp() {
    alphaUp = true;
    alphaDown = false;
  }

  public void goDown() {
    alphaUp = false;
    alphaDown = true;
  }
}
