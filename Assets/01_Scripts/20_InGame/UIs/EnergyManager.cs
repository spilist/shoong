﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyManager : MonoBehaviour {
  public static EnergyManager em;
  public GameObject dangerousFilter;
  public Image gauge;
  public GameObject gaugeShell;
  public GameObject gaugeIcon;
  private Image origGauge;
  private GameObject origGaugeShell;
  private GameObject origGaugeIcon;
  public float dangerousAt = 0.3f;
  private AudioSource takeDamageSound;
  private float changeTo = 0;
  private float changeRate;
  private float restAmount = 0;
  private float restRate;

  private bool energySystemOn = false;

  private bool isChanging = false;
  private bool isChangingRest = false;

  private float losePerSec;
  private float lessDamageRate;
  private float maxEnergy;
  private float energy;

  public float getRate = 2f;
  public float loseRate = 1;

  private Color color_healthy;
  private Color color_danger;
  private string lastReason;
  public bool noDeath = true;
  private float origEnergyWidth;
  private bool difficult;

  public bool killNow = false;

  public void setEnergyUI(Transform tr) {
    gauge = tr.Find("EnergyBarGuage").GetComponent<Image>();
    gaugeShell = tr.Find("EnergyBarShell").gameObject;
    gaugeIcon = tr.Find("EnergyBarIcon").gameObject;
    noDeath = true;
    energySystemOn = true;
    losePerSec = CharacterManager.cm.energyReduceOnTimeStandard;
    lessDamageRate = CharacterManager.cm.damageGetScaleStandard;
  }

  void Awake() {
    em = this;
    color_healthy = gauge.color;
    color_danger = new Color(1, 0, 0, color_healthy.a);
    takeDamageSound = transform.Find("TakeDamageSound").GetComponent<AudioSource>();

    origGauge = gauge;
    origGaugeShell = gaugeShell;
    origGaugeIcon = gaugeIcon;
    origEnergyWidth = gauge.GetComponent<RectTransform>().sizeDelta.x;
  }

  void Update () {
    if (energySystemOn && gauge.gameObject.activeInHierarchy) {
      if (gauge.fillAmount > dangerousAt) {
        gauge.color = color_healthy;
        dangerousFilter.SetActive(false);
      } else {
        gauge.color = color_danger;
        dangerousFilter.SetActive(true);
      }

      if (isChanging) {
        change();
      } else if (isChangingRest) {
        changeRest();
      } else {
        autoDecrease();
      }

      if (gauge.fillAmount == 0) {
        if (noDeath) getFullHealth();
        else {
          turnEnergy(false);
          if (isChangingRest && lastReason != "") {
            ScoreManager.sm.gameOver(lastReason);
          } else {
            ScoreManager.sm.gameOver("NoEnergy");
          }
        }
      }
      if (killNow == true) {
        killNow = false;
        turnEnergy(false);
        ScoreManager.sm.gameOver("NoEnergy");
      }
    }
	}

  public void turnEnergy(bool val) {
    if (val) {
      getFullHealth();
      // noDeath = false;
      gauge = origGauge;
      gaugeShell = origGaugeShell;
      gaugeIcon = origGaugeIcon;

      if (difficult) {
        losePerSec = CharacterManager.cm.energyReduceOnTimeStandard_hard;
        lessDamageRate = CharacterManager.cm.damageGetScaleStandard * 1.5f;
      } else {
        losePerSec = CharacterManager.cm.energyReduceOnTimeStandard;
        lessDamageRate = CharacterManager.cm.damageGetScaleStandard;
      }
    }

    gauge.gameObject.SetActive(val);
    gaugeShell.SetActive(val);
    gaugeIcon.SetActive(val);
    energySystemOn = val;
  }

  public void getFullHealth() {
    dangerousFilter.SetActive(false);
    gauge.fillAmount = 1;
    energy = maxEnergy;
    isChanging = false;
    isChangingRest = false;
    restAmount = 0;
  }

  void autoDecrease() {
    if (Player.pl.isUsingRainbow() || Player.pl.isUsingEMP() || Player.pl.isOnSuperheat()) return;

    energy = Mathf.MoveTowards(energy, 0, Time.deltaTime * losePerSec);
    gauge.fillAmount = energy / maxEnergy;
  }

  public void resetAbility() {
    maxEnergy = CharacterManager.cm.maxEnergyStandard;
    float changedWidth = origEnergyWidth * maxEnergy / 100.0f;
    float height = gauge.GetComponent<RectTransform>().sizeDelta.y;
    gauge.GetComponent<RectTransform>().sizeDelta = new Vector2(changedWidth, height);
    gaugeShell.GetComponent<RectTransform>().sizeDelta = new Vector2(changedWidth, height);
  }

  public void setDifficulty(bool difficult) {
    this.difficult = difficult;
  }

  void changeHealth (float amount, float rate) {
    if (!energySystemOn) return;

    if (isChanging) {
      restAmount += changeTo - gauge.fillAmount + amount / maxEnergy;
      restRate = changeRate;
    }

    isChanging = true;
    changeTo = gauge.fillAmount + amount / maxEnergy;
    changeTo = Mathf.Clamp(changeTo, 0, 1);
    changeRate = Time.deltaTime * rate;
  }

  void change() {
    energy = Mathf.MoveTowards(energy, maxEnergy * changeTo, changeRate * maxEnergy);
    gauge.fillAmount = energy / maxEnergy;

    float comparison = energy - maxEnergy * changeTo;
    if (Mathf.Round(comparison * 100f) / 100f == 0) {
      isChanging = false;
      if (restAmount != 0) {
        isChangingRest = true;
      }
    }
  }

  void changeRest() {
    if (restAmount != 0 && gauge.fillAmount != 1) {
      energy = Mathf.MoveTowards(energy, energy + restAmount * maxEnergy, restRate * maxEnergy);
      restAmount = Mathf.MoveTowards(restAmount, 0, restRate);
      gauge.fillAmount = energy / maxEnergy;
    } else {
      restAmount = 0;
      isChangingRest = false;
    }
  }

  public void getEnergy(float amount) {
    changeHealth(amount, getRate * amount);
    Player.pl.getEnergy.Play();
    // Player.pl.getEnergy.transform.GetChild(0).GetComponent<AudioSource>().Play();
  }

  public void loseEnergy(float amount, string tag) {
    if (amount <= 0) return;

    changeHealth(-amount * lessDamageRate, loseRate * amount * lessDamageRate);
    lastReason = tag;
    takeDamageSound.Play();
  }

  public int currentEnergy() {
    return (int) (gauge.fillAmount * 100);
  }
}

