using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour {
  private Image image;
  private float changeTo;
  private float changeRate;

  private bool isChanging;
  private bool isAuto;
  private bool gameStarted;
  private bool unstoppable = false;

  public float autoDecreaseRate = 0.05f;
  public int getAmount = 20;
	public int getAmountbyParts = 10;
  public float getRate = 2f;
  public int loseAmount = -20;
  public float loseRate = 0.5f;
  public int shootAmount = -10;
  public float shootRate = 0.25f;
  public GameOver gameOver;

  void Start () {
    image = GetComponent<Image>();
    isChanging = false;
    isAuto = false;
    gameStarted = false;
	}

	void Update () {
    if (gameStarted) {
      if (unstoppable) return;

      if (isAuto) {
        autoDecrease();
      }
      else if (isChanging) {
        change();
      }

      if (image.fillAmount == 0) {
        gameOver.run();
        gameStarted = false;
      }
    }
	}

  void autoDecrease() {
    image.fillAmount = Mathf.MoveTowards(image.fillAmount, 0, Time.deltaTime * autoDecreaseRate);
  }

  void change() {
    image.fillAmount = Mathf.MoveTowards(image.fillAmount, changeTo, changeRate);

    if (image.fillAmount == changeTo) {
      isChanging = false;
      isAuto = true;
    }
  }

  public void startDecrease() {
    gameStarted = true;
    isAuto = true;
  }

  public void getHealth() {
    changeHealth(getAmount, getRate);
  }

	public void getHealthbyParts() {
		changeHealth(getAmountbyParts, getRate);
	}

  public void loseHealth() {
    changeHealth(loseAmount, loseRate);
  }

  public void loseByShoot() {
    changeHealth(shootAmount, shootRate);
  }

  void changeHealth (int amount, float rate) {
    if (!gameStarted)
      return;

    isChanging = true;
    isAuto = false;
    changeTo = image.fillAmount + amount / 100f;
    if (amount > 0) {
      changeTo = Mathf.Min(1f, changeTo);
    }
    else {
      changeTo = Mathf.Max(0, changeTo);
    }
    changeRate = Time.deltaTime * rate;
  }

  public void startUnstoppable() {
    unstoppable = true;
  }

  public void stopUnstoppable() {
    unstoppable = false;
  }
}

