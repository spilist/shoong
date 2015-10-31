using UnityEngine;
using System.Collections;

public class RhythmRing : MonoBehaviour {
  public bool skillRing = false;
  private bool originalSkillRing;
  public bool feverRing = false;
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
  bool minMsgSended = false;
  bool disappearing = false;
  bool afterMin = false;
  float alpha;
  Color color;
  private SpriteRenderer sRenderer;

  void Awake() {
    startScale = transform.localScale.x;
    originalSkillRing = skillRing;
    sRenderer = GetComponent<SpriteRenderer>();
    originalColor = sRenderer.color;

    minBoosterOkScale = RhythmManager.rm.minBoosterOkScale;
    maxBoosterOkScale = RhythmManager.rm.maxBoosterOkScale;
    minPopScale = RhythmManager.rm.minPopScale;
    maxPopScale = RhythmManager.rm.maxPopScale;
    rightBeatScale = RhythmManager.rm.rightBeatScale;
    disappearDuration = RhythmManager.rm.ringDisppearDuration;
    playerScaleUpAmount = RhythmManager.rm.playerScaleUpAmount;
  }

  void OnEnable() {
    sRenderer.enabled = true;
    transform.localScale = startScale * Vector3.one;
    sRenderer.color = originalColor;
    scale = startScale;
    color = originalColor;
    alpha = 1;
    maxMsgSended = false;
    minMsgSended = false;
    disappearing = false;
    afterMin = false;
    skillRing = originalSkillRing;

    beat = RhythmManager.rm.samplePeriod;
  }

  void Update() {
    if (disappearing) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / disappearDuration);
      color.a = alpha;
      sRenderer.color = color;
      if (alpha == 0) {
        disappearing = false;
        gameObject.SetActive(false);
      }
    } else {
      scale = Mathf.MoveTowards(scale, 0, Time.deltaTime * (startScale - rightBeatScale) / beat);
      transform.localScale = scale * Vector3.one;

      if (!feverRing) {
        if (!maxMsgSended && scale <= maxBoosterOkScale) {
          maxMsgSended = true;
          RhythmManager.rm.boosterOk(true, skillRing);
        }

        if (sRenderer.enabled && scale <= maxPopScale) {
          sRenderer.enabled = false;
          Player.pl.scaleChange(true, playerScaleUpAmount);
        }

        if (!afterMin && scale <= minPopScale) {
          afterMin = true;
          Player.pl.scaleChange(false, playerScaleUpAmount);
        }

        if (!minMsgSended && scale <= minBoosterOkScale) {
          minMsgSended = true;
          RhythmManager.rm.boosterOk(false, false);
          RhythmManager.rm.afterRing(true);
        }
      }

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
    RhythmManager.rm.afterRing(false);
  }
}
