using UnityEngine;
using System.Collections;

public class BannerButton : MenusBehavior {
  protected GameObject gameOverUI;
  protected BackButton back;
  public string description;

  void Start() {
    gameOverUI = GameObject.Find("GameOver").gameObject;
    back = gameOverUI.GetComponent<ScoreUpdate>().back;
  }

  virtual public bool available() {
    return false;
  }

  virtual public void goBack() {

  }
}
