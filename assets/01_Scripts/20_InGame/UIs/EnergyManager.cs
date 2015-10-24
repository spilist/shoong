using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyManager : MonoBehaviour {
  public static EnergyManager energy;
  public Image gauge;
  public GameObject gaugeShell;
  public GameObject gaugeIcon;
  private float changeTo = 0;
  private float changeRate;
  private bool energySystemOn = false;

  private bool isChanging = false;

  public float autoDecreaseRate = 0.05f;
  public int getAmountbyCubes = 10;
  public float getRate = 2f;
  public PlayerMover player;

  private Color color_healthy;
  private Color color_danger;

  void Awake() {
    energy = this;
    color_healthy = gauge.color;
    color_danger = new Color(1, 0, 0, color_healthy.a);
  }

  void Update () {
    if (energySystemOn) {
      if (gauge.fillAmount > 0.3f) {
        gauge.color = color_healthy;
      } else {
        gauge.color = color_danger;
      }

      if (isChanging) {
        change();
      } else {
        autoDecrease();
      }

      if (gauge.fillAmount == 0) {
        turnEnergy(false);
        if (player.isTrapped()) {
          ScoreManager.sm.gameOver("Trap");
        } else {
          ScoreManager.sm.gameOver("NoEnergy");
        }
      }
    }
	}

  public void turnEnergy(bool val) {
    gauge.gameObject.SetActive(val);
    gaugeShell.SetActive(val);
    gaugeIcon.SetActive(val);
    energySystemOn = val;

    if (val) getFullHealth();
  }

  public void getFullHealth() {
    gauge.fillAmount = 1;
    isChanging = false;
  }

  void autoDecrease() {
    if (player.isUsingRainbow() || player.isUsingEMP() || player.isOnSuperheat()) return;

    gauge.fillAmount = Mathf.MoveTowards(gauge.fillAmount, 0, Time.deltaTime * autoDecreaseRate);
  }

  void change() {
    gauge.fillAmount = Mathf.MoveTowards(gauge.fillAmount, changeTo, changeRate);

    if (gauge.fillAmount == changeTo) {
      isChanging = false;
    }
  }

  void changeHealth (int amount, float rate) {
    isChanging = true;
    changeTo = gauge.fillAmount + amount / 100f;
    changeTo = Mathf.Clamp(changeTo, 0, 1);
    changeRate = Time.deltaTime * rate;
  }

  public void getHealthByCubes(int amount) {
    changeHealth(getAmountbyCubes * amount, getRate * amount);
  }

  public int currentEnergy() {
    return (int) (gauge.fillAmount * 100);
  }
}

