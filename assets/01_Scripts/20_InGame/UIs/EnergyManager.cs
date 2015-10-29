﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyManager : MonoBehaviour {
  public static EnergyManager em;
  public Image gauge;
  public GameObject gaugeShell;
  public GameObject gaugeIcon;
  public float dangerousAt = 0.3f;
  private AudioSource takeDamageSound;
  private float changeTo = 0;
  private float changeRate;
  private float restAmount = 0;
  private float restRate;

  private bool energySystemOn = false;

  private bool isChanging = false;
  private bool isChangingRest = false;

  public float mustDieIn = 15;
  private float originalDieIn;
  public int getAmountbyCubes = 10;
  public float getRate = 2f;
  public float loseRate = 1;
  private float lessDamageRate = 1;

  private Color color_healthy;
  private Color color_danger;
  private string lastReason;

  void Awake() {
    em = this;
    color_healthy = gauge.color;
    color_danger = new Color(1, 0, 0, color_healthy.a);
    originalDieIn = mustDieIn;
    takeDamageSound = transform.Find("TakeDamageSound").GetComponent<AudioSource>();
  }

  void Update () {
    if (energySystemOn) {
      if (gauge.fillAmount > dangerousAt) {
        gauge.color = color_healthy;
      } else {
        gauge.color = color_danger;
      }

      if (isChanging) {
        change();
      } else if (isChangingRest) {
        changeRest();
      } else {
        autoDecrease();
      }

      if (gauge.fillAmount == 0) {
        turnEnergy(false);
        if (Player.pl.isTrapped()) {
          ScoreManager.sm.gameOver("Trap");
        } else if (isChangingRest && lastReason != "") {
          ScoreManager.sm.gameOver(lastReason);
        } else {
          ScoreManager.sm.gameOver("NoEnergy");
        }
      }
    }
	}

  public void turnEnergy(bool val) {
    gauge.gameObject.SetActive(val);
    gaugeShell.SetActive(val);
    // gaugeIcon.SetActive(val);
    energySystemOn = val;

    if (val) getFullHealth();
  }

  public void getFullHealth() {
    gauge.fillAmount = 1;
    isChanging = false;
    isChangingRest = false;
    restAmount = 0;
  }

  void autoDecrease() {
    if (Player.pl.isUsingRainbow() || Player.pl.isUsingEMP() || Player.pl.isOnSuperheat()) return;

    gauge.fillAmount = Mathf.MoveTowards(gauge.fillAmount, 0, Time.deltaTime / mustDieIn);
  }

  public void moreEnergyEfficiency(int val) {
    mustDieIn *= (100 + val) / 100f;
  }

  public void lessDamage(int val) {
    lessDamageRate *= (100 - val) / 100f;
  }

  public void resetEnergyAbility() {
    mustDieIn = originalDieIn;
    lessDamageRate = 1;
  }

  void change() {
    gauge.fillAmount = Mathf.MoveTowards(gauge.fillAmount, changeTo, changeRate);

    if (gauge.fillAmount == changeTo) {
      isChanging = false;
      isChangingRest = true;
    }
  }

  void changeHealth (float amount, float rate) {
    if (!energySystemOn) return;

    if (isChanging) {
      restAmount += changeTo - gauge.fillAmount + amount / 100f;
      restRate = changeRate;
    }

    isChanging = true;
    changeTo = gauge.fillAmount + amount / 100f;
    changeTo = Mathf.Clamp(changeTo, 0, 1);
    changeRate = Time.deltaTime * rate;
  }

  void changeRest() {
    if (restAmount != 0 && gauge.fillAmount != 1) {
      gauge.fillAmount = Mathf.MoveTowards(gauge.fillAmount, gauge.fillAmount + restAmount, restRate);
      restAmount = Mathf.MoveTowards(restAmount, 0, restRate);
    } else {
      restAmount = 0;
      isChangingRest = false;
    }
  }

  public void getHealthByCubes(int amount) {
    changeHealth(getAmountbyCubes * amount, getRate * amount);
  }

  public void loseEnergy(int amount, string tag) {
    changeHealth(-amount * lessDamageRate, loseRate * amount * lessDamageRate);
    lastReason = tag;
    takeDamageSound.Play();
  }

  public int currentEnergy() {
    return (int) (gauge.fillAmount * 100);
  }
}

