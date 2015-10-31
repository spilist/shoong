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
  float rightBeatScale;
  bool maxMsgSended = false;
  bool minMsgSended = false;
  private SpriteRenderer sRenderer;

  void Awake() {
    startScale = transform.localScale.x;
    originalSkillRing = skillRing;
    sRenderer = GetComponent<SpriteRenderer>();
    originalColor = sRenderer.color;
  }

  void OnEnable() {
    transform.localScale = startScale * Vector3.one;
    sRenderer.color = originalColor;
    scale = startScale;
    maxMsgSended = false;
    minMsgSended = false;
    skillRing = originalSkillRing;
    minBoosterOkScale = RhythmManager.rm.minBoosterOkScale;
    maxBoosterOkScale = RhythmManager.rm.maxBoosterOkScale;
    rightBeatScale = RhythmManager.rm.rightBeatScale;

    beat = RhythmManager.rm.samplePeriod;
  }

  void Update() {
    scale = Mathf.MoveTowards(scale, 0, Time.deltaTime * (startScale - rightBeatScale) / beat);
    transform.localScale = scale * Vector3.one;

    if (!feverRing) {
      if (!maxMsgSended && scale <= maxBoosterOkScale) {
        maxMsgSended = true;
        RhythmManager.rm.boosterOk(true, skillRing);
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

  void OnDisable() {
    RhythmManager.rm.afterRing(false);
  }
}
