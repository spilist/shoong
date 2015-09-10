﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour {
  private Image image;
  private float changeTo = 0;
  private float changeRate;
  private float restAmount = 0;
  private float restRate;

  private bool isChanging = false;
  private bool isChangingRest = false;
  private bool gameStarted = false;
  private bool charged = true;
  public Transform chargeEffect;
  public float originalChargeEffectScale = 5;
  private float chargeEffectScale;
  public float chargingEffectDuration = 0.5f;
  private float chargedCount = 0;
  public float chargedDuration = 2.5f;

  public float autoDecreaseRate = 0.05f;
  public int getAmountbyParts = 10;
  public float getRate = 2f;
  public int shootAmount = -10;
  public float loseRate = 1;
  public PlayerMover player;
  public Text energyCurrent;
  public GameObject loseEnergy;
  public GameObject getEnergy;

  private Color color_healthy;
  private Color color_danger;

  void Start () {
    image = GetComponent<Image>();

    color_healthy = image.color;
    color_danger = new Color(1, 0, 0, color_healthy.a);
    chargeEffectScale = originalChargeEffectScale;
    chargeEffect.localScale = originalChargeEffectScale * Vector3.one;
  }

  void Update () {
    if (gameStarted) {
      if (charged) {
        if (chargedCount < chargingEffectDuration) {
          chargeEffectScale = Mathf.MoveTowards(chargeEffectScale, 1, Time.deltaTime * (originalChargeEffectScale - 1) / chargingEffectDuration);
          chargeEffect.localScale = chargeEffectScale * Vector3.one;
        }

        if (chargedCount >= chargedDuration) {
          chargedCount = 0;
          charged = false;
        }
        chargedCount += Time.deltaTime;
      } else {
        if (isChanging) {
          change();
        } else if (isChangingRest) {
          changeRest();
        } else {
          autoDecrease();
        }

        if (image.fillAmount > 0.3f) {
          image.color = color_healthy;
        } else {
          image.color = color_danger;
        }

        if (image.fillAmount == 0) {
          player.scoreManager.gameOver();
          gameStarted = false;
        }
      }
      energyCurrent.text = (image.fillAmount * 100).ToString("0");
    }
	}

  public void getFullHealth() {
    image.fillAmount = 1;
    isChanging = false;
    isChangingRest = false;
    restAmount = 0;
  }

  void autoDecrease() {
    if (player.isUsingRainbow()) return;
    image.fillAmount = Mathf.MoveTowards(image.fillAmount, 0, Time.deltaTime * autoDecreaseRate);
  }

  void change() {
    image.fillAmount = Mathf.MoveTowards(image.fillAmount, changeTo, changeRate);

    if (image.fillAmount == changeTo) {
      isChanging = false;
      isChangingRest = true;
    }
  }

  void changeRest() {
    if (restAmount != 0 && image.fillAmount != 1) {
      image.fillAmount = Mathf.MoveTowards(image.fillAmount, image.fillAmount + restAmount, restRate);
      restAmount = Mathf.MoveTowards(restAmount, 0, restRate);
    } else {
      restAmount = 0;
      isChangingRest = false;
    }
  }

  public void startGame() {
    gameStarted = true;
    player.chargedEffect.Play();
    player.chargedEffect.GetComponent<AudioSource>().Play();
  }

  void changeHealth (int amount, float rate) {
    if (!gameStarted)
      return;

    // if it is the middle of change, save the rest amount and rate
    if (isChanging) {
      restAmount += changeTo - image.fillAmount + amount / 100f;
      restRate = changeRate;
    }

    isChanging = true;
    changeTo = image.fillAmount + amount / 100f;
    changeTo = Mathf.Clamp(changeTo, 0, 1);
    changeRate = Time.deltaTime * rate;

    GameObject showEnergyInstance;
    if (amount > 0) {
      showEnergyInstance = (GameObject) Instantiate(getEnergy);
    } else {
      showEnergyInstance = (GameObject) Instantiate(loseEnergy);
    }
      showEnergyInstance.transform.SetParent(transform.parent.transform, false);
      showEnergyInstance.GetComponent<ShowChangeText>().run(amount);
  }

  public void getHealthbyParts() {
    changeHealth(getAmountbyParts, getRate);
  }

  public void getHealthbyParts(int combo) {
    changeHealth(getAmountbyParts * combo, getRate * combo);
  }

  public void loseByShoot() {
    if (charged) return;
    changeHealth(shootAmount, loseRate);
  }

  public void setCharged() {
    charged = true;
  }

  public int currentEnergy() {
    return (int) (image.fillAmount * 100);
  }
}

