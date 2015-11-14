using UnityEngine;
using System.Collections;

public class RhythmStar : MonoBehaviour {
  public bool skillRing = false;
  private bool originalSkillRing;
  Color originalColor;
  Vector3 originalPos;
  float startPosX;
  float posX;
  float beat;
  float minBoosterOkPosX;
  float maxBoosterOkPosX;
  float rightBeatPosX;
  float disappearDuration;
  bool disappearing = false;
  bool rightMsgSended = false;
  bool maxMsgSended = false;
  bool minMsgSended = false;
  float alpha;
  Color color;
  private SpriteRenderer sRenderer;
  float timePeriod;
  ParticleSystem particle;

  void Awake() {
    originalPos = transform.localPosition;
    startPosX = originalPos.x;
    originalSkillRing = skillRing;
    sRenderer = GetComponent<SpriteRenderer>();
    originalColor = sRenderer.color;
    particle = GetComponent<ParticleSystem>();

    rightBeatPosX = RhythmManager.rm.rightBeatPosX;
    minBoosterOkPosX = rightBeatPosX * RhythmManager.rm.minBoosterOkPosX;
    maxBoosterOkPosX = rightBeatPosX * RhythmManager.rm.maxBoosterOkPosX;
    disappearDuration = RhythmManager.rm.ringDisppearDuration;
  }

  void OnEnable() {
    sRenderer.enabled = true;
    transform.localPosition = originalPos;
    sRenderer.color = originalColor;
    posX = startPosX;
    color = originalColor;
    alpha = 1;
    maxMsgSended = false;
    minMsgSended = false;
    disappearing = false;
    rightMsgSended = false;

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
      timePeriod = 100000 * Time.deltaTime * Mathf.Abs(startPosX - rightBeatPosX);
      timePeriod /= beat;
      timePeriod /= 100000;
      posX = Mathf.MoveTowards(posX, minBoosterOkPosX, timePeriod);
      transform.localPosition = new Vector3(posX, originalPos.y, originalPos.z);

      if (!maxMsgSended && posX >= maxBoosterOkPosX) {
        maxMsgSended = true;
        RhythmManager.rm.setCurrent(this);
        RhythmManager.rm.boosterOk(true, skillRing);
      }

      if (!rightMsgSended && posX >= rightBeatPosX) {
        rightMsgSended = true;
        // sRenderer.enabled = false;
        // RhythmManager.rm.afterRing(true);
      }

      if (!minMsgSended && posX >= minBoosterOkPosX) {
        minMsgSended = true;
        gameObject.SetActive(false);
        RhythmManager.rm.boosterOk(false, false);
        RhythmManager.rm.ringSkipped(skillRing);
      }
    }
  }

  public void disappear() {
    disappearing = true;
  }

  public void success() {
    disappearing = true;
    particle.Play();
  }
}
