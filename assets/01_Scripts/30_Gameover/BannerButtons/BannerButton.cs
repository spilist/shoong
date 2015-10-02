using UnityEngine;
using System.Collections;

public class BannerButton : MenusBehavior {
  protected GameObject gameOverUI;
  protected BackButton back;
  public string description;
  public int requiredSpace = 1;
  public bool picked = false;

  void Start() {
    gameOverUI = GameObject.Find("GameOver").gameObject;
    back = gameOverUI.GetComponent<ScoreUpdate>().back;
    initRest();
  }

  virtual protected void initRest() {
  }

  virtual public bool available(int spaceLeft) {
    return false;
  }

  virtual public void goBack() {

  }
}
