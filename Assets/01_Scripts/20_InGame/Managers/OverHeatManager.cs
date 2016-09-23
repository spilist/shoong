using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverHeatManager : MonoBehaviour {
  public static OverHeatManager ohm;

  public Image icon;
  public Image glow;
  public float dimmedAlpha = 0.35f;
  public float brightAlpha = 0.7f;
  private Color originalColor;
  private Color dimmedColor;
  private Color brightColor;
  private TrailRenderer trail;
  public GameObject trailParticle;

  public Text currentGaugeText;
  public Text percentText;
  public Color textDimmedColor;
  public Color textBrightColor;

  private float currentGauge = 0;
  private float totalGauge = 0;
  public int increaseSpeed = 5;

  public Text maxGaugeText;
  public int maxGauge = 100;
  public float gaugePerPoints = 0.1f;
  public float gaugeReducePerSecond = 1f;
  public int gaugeTurnOffAt = 70;
  public float stayAtMaxDuration = 3f;

  public float characterSpeedIncrease = 1.15f;
  public float increasePitchAmount = 1.3f;

  public bool onOverHeat = false;
  private bool gaugeStayingMax = false;

	void Awake() {
    ohm = this;
    maxGaugeText.text = "/" + maxGauge.ToString("0");

    originalColor = icon.color;
    dimmedColor = new Color(originalColor.r, originalColor.g, originalColor.b, dimmedAlpha);
    brightColor = new Color(originalColor.r, originalColor.g, originalColor.b, brightAlpha);

    icon.color = dimmedColor;
    glow.color = dimmedColor;
  }

  void Start () {
    trail = Player.pl.GetComponent<TrailRenderer>();
	}

	void Update () {
    if (currentGauge != totalGauge) {
      currentGauge = Mathf.MoveTowards(currentGauge, totalGauge, Time.deltaTime * Mathf.Abs(totalGauge - currentGauge) * increaseSpeed);
      glow.fillAmount = currentGauge / maxGauge;
    }

    currentGaugeText.text = currentGauge.ToString("0");
	}

  public void addGauge(int amount) {
    totalGauge += amount * gaugePerPoints;

    if (totalGauge >= maxGauge) {
      totalGauge = maxGauge;
      startOverHeat();
    }
  }

  public void reduceGaugeByTime() {
    if (gaugeStayingMax) return;

    if (totalGauge <= gaugeReducePerSecond) {
      totalGauge = 0;
      return;
    }

    totalGauge -= gaugeReducePerSecond;

    if (totalGauge <= gaugeTurnOffAt) {
      stopOverHeat();
    }
  }

  private void startOverHeat() {
    if (!onOverHeat) {
      onOverHeat = true;
      gaugeStayingMax = true;
      icon.color = brightColor;
      glow.color = brightColor;

      trail.enabled = true;
      trail.time = 1;
      trailParticle.SetActive(true);

      currentGaugeText.color = textBrightColor;
      percentText.color = textBrightColor;

      StartCoroutine(changeGaugeStaingMax());
      StartCoroutine(overheatSoundEffect());
    }
  }

  IEnumerator changeGaugeStaingMax() {
    yield return new WaitForSeconds(stayAtMaxDuration);

    gaugeStayingMax = false;
  }

  private void stopOverHeat() {
    onOverHeat = false;

    trail.enabled = false;
    trail.time = 0;
    trailParticle.SetActive(false);

    icon.color = dimmedColor;
    glow.color = dimmedColor;

    currentGaugeText.color = textDimmedColor;
    percentText.color = textDimmedColor;
  }

  IEnumerator overheatSoundEffect() {
    yield return null;
    float moveAmount = AudioManager.am.main.movePitchToPercent(increasePitchAmount, 0f);
    Debug.Log("MoveAmount: " + moveAmount);
    while (onOverHeat) {
      yield return null;
    }
    AudioManager.am.main.movePitch(-moveAmount, 0.5f);
  }
}
