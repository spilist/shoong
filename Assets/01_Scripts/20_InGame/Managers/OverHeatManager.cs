using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverHeatManager : MonoBehaviour {
  public static OverHeatManager ohm;

  public Text currentGaugeText;
  private float currentGauge = 0;
  private float totalGauge = 0;
  public int increaseSpeed = 5;

  public Text maxGaugeText;
  public int maxGauge = 100;
  public float gaugePerPoints = 0.1f;
  public float gaugeReducePerSecond = 1f;
  public int gaugeTurnOffAt = 70;

  public float characterSpeedIncrease = 1.15f;
  public float increasePitchAmount = 1.3f;

  public bool onOverHeat = false;

	void Awake() {
    ohm = this;
    maxGaugeText.text = "/" + maxGauge.ToString("0");
  }

  void Start () {

	}

	void Update () {
    if (currentGauge != totalGauge) {
      currentGauge = Mathf.MoveTowards(currentGauge, totalGauge, Time.deltaTime * Mathf.Abs(totalGauge - currentGauge) * increaseSpeed);
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
      StartCoroutine(overheatSoundEffect());
    }
  }

  private void stopOverHeat() {
    onOverHeat = false;
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
