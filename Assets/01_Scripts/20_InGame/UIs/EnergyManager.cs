using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyManager : MonoBehaviour {
  public static EnergyManager em;
  public GameObject dangerousFilter;
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

  private float mustDieIn;
  private float lessDamageRate;
  private float maxEnergy;
  private float energy;

  public float getRate = 2f;
  public float loseRate = 1;

  private Color color_healthy;
  private Color color_danger;
  private string lastReason;

  void Awake() {
    em = this;
    color_healthy = gauge.color;
    color_danger = new Color(1, 0, 0, color_healthy.a);
    takeDamageSound = transform.Find("TakeDamageSound").GetComponent<AudioSource>();
  }

  void Update () {
    if (energySystemOn) {
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
        turnEnergy(false);
        if (isChangingRest && lastReason != "") {
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
    gaugeIcon.SetActive(val);
    energySystemOn = val;

    if (val) getFullHealth();
  }

  public void getFullHealth() {
    gauge.fillAmount = 1;
    energy = maxEnergy;
    isChanging = false;
    isChangingRest = false;
    restAmount = 0;
  }

  void autoDecrease() {
    if (Player.pl.isUsingRainbow() || Player.pl.isUsingEMP() || Player.pl.isOnSuperheat()) return;

    energy = Mathf.MoveTowards(energy, 0, Time.deltaTime * maxEnergy / mustDieIn);
    gauge.fillAmount = energy / maxEnergy;
  }

  public void resetAbility() {
    mustDieIn = CharacterManager.cm.energyReduceOnTimeStandard;
    lessDamageRate = CharacterManager.cm.damageGetScaleStandard;

    maxEnergy = CharacterManager.cm.maxEnergyStandard;
    float changedWidth = gauge.GetComponent<RectTransform>().sizeDelta.x * maxEnergy / 100.0f;
    float height = gauge.GetComponent<RectTransform>().sizeDelta.y;
    gauge.GetComponent<RectTransform>().sizeDelta = new Vector2(changedWidth, height);
    gaugeShell.GetComponent<RectTransform>().sizeDelta = new Vector2(changedWidth, height);
  }

  void change() {
    energy = Mathf.MoveTowards(energy, maxEnergy * changeTo, changeRate * maxEnergy);
    gauge.fillAmount = energy / maxEnergy;

    if (gauge.fillAmount == changeTo) {
      isChanging = false;
      isChangingRest = true;
    }
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

  void changeRest() {
    if (restAmount != 0 && gauge.fillAmount != 1) {
      energy = Mathf.MoveTowards(energy, energy + restAmount * maxEnergy, restRate * maxEnergy);
      restAmount = Mathf.MoveTowards(restAmount, 0, restRate);
    } else {
      restAmount = 0;
      isChangingRest = false;
    }
  }

  public void getEnergy(int amount) {
    changeHealth(amount, getRate * amount);
    Player.pl.getEnergy.Play();
    Player.pl.getEnergy.GetComponent<AudioSource>().Play();
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

