using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmStar : MonoBehaviour {
  public bool skillRing = false;
  private bool originalSkillRing;
  Color originalColor;
  Vector2 originalPos;
  float startPosX;
  float posX;
  float beat;
  float canBeMissedPosX;
  float minBoosterOkPosX;
  float maxBoosterOkPosX;
  float rightBeatPosX;
  float disappearDuration;
  bool disappearing = false;
  bool rightMsgSended = false;
  bool maxMsgSended = false;
  bool minMsgSended = false;
  bool missMsgSended = false;
  float alpha;
  Color color;
  private Image image;
  float timePeriod;
  ParticleSystem particle;
  AudioSource audioSource;
  RectTransform rect;

  void Awake() {
    rect = GetComponent<RectTransform>();
    originalPos = rect.anchoredPosition;
    startPosX = originalPos.x;

    originalSkillRing = skillRing;
    image = GetComponent<Image>();
    originalColor = image.color;
    particle = GetComponent<ParticleSystem>();
    audioSource = GetComponent<AudioSource>();

    rightBeatPosX = RhythmManager.rm.rightBeatPosX;
    canBeMissedPosX = RhythmManager.rm.canBeMissedPosX;
    disappearDuration = RhythmManager.rm.ringDisppearDuration;
    beat = RhythmManager.rm.samplePeriod;
  }

  void OnEnable() {
    image.enabled = true;
    rect.anchoredPosition = originalPos;
    image.color = originalColor;
    posX = startPosX;
    color = originalColor;
    alpha = 0;
    maxMsgSended = false;
    minMsgSended = false;
    disappearing = false;
    rightMsgSended = false;
    missMsgSended = false;

    minBoosterOkPosX = RhythmManager.rm.minOK();
    maxBoosterOkPosX = RhythmManager.rm.maxOK();

    skillRing = originalSkillRing;
  }

  void Update() {
    if (disappearing) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / disappearDuration);
      color.a = alpha;
      image.color = color;
      if (alpha == 0) {
        disappearing = false;
        gameObject.SetActive(false);
      }
    } else {
      timePeriod = 100000 * Time.deltaTime * Mathf.Abs(startPosX - rightBeatPosX);
      timePeriod /= beat;
      timePeriod /= 100000;
      posX = Mathf.MoveTowards(posX, maxBoosterOkPosX, timePeriod);
      rect.anchoredPosition = new Vector2(posX, originalPos.y);

      if (!missMsgSended && posX >= canBeMissedPosX) {
        missMsgSended = true;
        RhythmManager.rm.setCurrent(this);
      }

      if (!minMsgSended && posX >= minBoosterOkPosX) {
        minMsgSended = true;
        RhythmManager.rm.setCanBeMissed(false);
        RhythmManager.rm.boosterOk(true, skillRing);
      }

      if (!rightMsgSended && posX >= rightBeatPosX) {
        rightMsgSended = true;

        Player.pl.shootBooster();
        // image.enabled = false;
        // RhythmManager.rm.afterRing(true);
      }

      if (!maxMsgSended && posX >= maxBoosterOkPosX) {
        maxMsgSended = true;
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
    // particle.Play();
    // if (audioSource != null && audioSource.enabled) audioSource.Play();
  }
}
