using UnityEngine;
using System.Collections;
using SmartLocalization;

public class BannerButton : MenusBehavior {
  protected GameObject gameOverUI;
  protected BackButton back;
  protected GameOverGoldCubes gold;
  public string firstKey;
  public string secondKey;
  protected string description;
  protected string secondDescription;
  public bool picked = false;
  public bool popping = true;
  public float startScale = 4;
  public float popScale = 1.5f;
  public float changeTime = 0.15f;
  private int popStatus = 0;
  private float scale;

  void Start() {
    gameOverUI = GameObject.Find("GameOver").gameObject;
    back = gameOverUI.GetComponent<ScoreUpdate>().back;
    gold = GameObject.Find("GameOverGoldCubes").GetComponent<GameOverGoldCubes>();
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;
    OnChangeLanguage(languageManager);
    initRest();
  }

  public void startPop() {
    if (popping) {
      scale = startScale;
      popStatus++;
    }
  }

  virtual protected void initRest() {
  }

  virtual public bool available() {
    return false;
  }

  virtual public void goBack() {

  }

  public string getDescription() {
    return description;
  }

  virtual protected void Update() {
    if (popStatus > 0) {
      if (popStatus == 1) {
        changeScale(popScale * startScale, popScale * startScale - startScale);
      } else if (popStatus == 2) {
        changeScale(startScale, popScale * startScale - startScale);
      }

      transform.localScale = scale * Vector3.one;
    }
  }

  void changeScale(float targetScale, float difference) {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (scale == targetScale) popStatus++;
  }

  void OnChangeLanguage(LanguageManager languageManager) {
    description = LanguageManager.Instance.GetTextValue(firstKey);
    if (secondKey != "") secondDescription = LanguageManager.Instance.GetTextValue(secondKey);
  }

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }
}
