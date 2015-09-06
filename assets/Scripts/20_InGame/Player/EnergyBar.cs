using UnityEngine;
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
	}

	void Update () {
    if (gameStarted) {
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

      energyCurrent.text = (image.fillAmount * 100).ToString("0");

      if (image.fillAmount == 0) {
        player.scoreManager.gameOver();
        gameStarted = false;
      }
    }
	}

  public void getFullHealth() {
    image.fillAmount = 1;
    isChanging = false;
    isChangingRest = false;
    restAmount = 0;
  }

  void autoDecrease() {
    if (player.isUnstoppable() || player.isUsingRainbow()) return;
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

  public void startDecrease() {
    gameStarted = true;
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
    changeHealth(shootAmount, loseRate);
  }

  public int currentEnergy() {
    return (int) (image.fillAmount * 100);
  }
}

