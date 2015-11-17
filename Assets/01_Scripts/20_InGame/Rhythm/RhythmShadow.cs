using UnityEngine;
using System.Collections;

public class RhythmShadow : MonoBehaviour {
  public bool skillRing = false;
  private bool originalSkillRing;
  Color originalColor;
  float beat;
  float startScale;
  float scale;
  float minBoosterOkScale;
  float maxBoosterOkScale;
  float minPopScale;
  float maxPopScale;
  float rightBeatScale;
  float disappearDuration;
  float playerScaleUpAmount;
  bool maxMsgSended = false;
  bool rightMsgSended = false;
  bool minMsgSended = false;
  bool disappearing = false;
  float alpha;
  Color color;
  private MeshRenderer sRenderer;

  void Awake() {
    startScale = transform.localScale.x;
    originalSkillRing = skillRing;
    sRenderer = GetComponent<MeshRenderer>();
    originalColor = sRenderer.sharedMaterial.color;

    // minBoosterOkScale = startScale * RhythmManager.rm.minBoosterOkScale / RhythmManager.rm.scaleBase;
    // maxBoosterOkScale = startScale * RhythmManager.rm.maxBoosterOkScale / RhythmManager.rm.scaleBase;
    // minPopScale = startScale * RhythmManager.rm.minPopScale / RhythmManager.rm.scaleBase;
    // maxPopScale = startScale * RhythmManager.rm.maxPopScale / RhythmManager.rm.scaleBase;
    // rightBeatScale = startScale * RhythmManager.rm.rightBeatScale / RhythmManager.rm.scaleBase;
    disappearDuration = RhythmManager.rm.ringDisppearDuration;
    // playerScaleUpAmount = RhythmManager.rm.playerScaleUpAmount;
  }

  void OnEnable() {
    GetComponent<MeshFilter>().sharedMesh = Player.pl.GetComponent<MeshFilter>().sharedMesh;
    transform.rotation = Player.pl.transform.rotation;
    // sRenderer.enabled = true;
    transform.localScale = startScale * Vector3.one;
    sRenderer.sharedMaterial.color = originalColor;
    scale = startScale;
    color = originalColor;
    alpha = 1;
    maxMsgSended = false;
    rightMsgSended = false;
    minMsgSended = false;
    disappearing = false;
    skillRing = originalSkillRing;

    beat = RhythmManager.rm.samplePeriod;
  }

  void Update() {
    transform.rotation = Player.pl.transform.rotation;

    if (disappearing) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / disappearDuration);
      color.a = alpha;
      sRenderer.material.color = color;
      if (alpha == 0) {
        disappearing = false;
        gameObject.SetActive(false);
      }
    } else {
      scale = Mathf.MoveTowards(scale, 0, Time.deltaTime * (startScale - rightBeatScale) / beat);
      transform.localScale = scale * Vector3.one;
      if (!maxMsgSended && scale <= maxBoosterOkScale) {
        maxMsgSended = true;
        RhythmManager.rm.boosterOk(true, skillRing);
      }

      if (!rightMsgSended && scale <= rightBeatScale) {
        rightMsgSended = true;
      }

      if (!minMsgSended && scale <= minBoosterOkScale) {
        minMsgSended = true;
        RhythmManager.rm.boosterOk(false, false);
        // RhythmManager.rm.afterRing(true);
        sRenderer.enabled = false;
      }

      // if (sRenderer.enabled && scale <= maxPopScale) {
      //   Player.pl.scaleChange(true, playerScaleUpAmount);
      // }

      // if (!afterMin && scale <= minPopScale) {
      //   Player.pl.scaleChange(false, playerScaleUpAmount);
      // }

      if (scale == 0) {
        RhythmManager.rm.ringSkipped(skillRing);
        gameObject.SetActive(false);
      }
    }
  }

  public void disappear() {
    disappearing = true;
  }

  void OnDisable() {
    // RhythmManager.rm.afterRing(false);
  }
}
