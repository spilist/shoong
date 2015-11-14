using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class GetPartsText : MonoBehaviour {
  public int limit;
  public TutorialHandler tutoHandler;
  private Text text;
  private string description;
  private int count = 0;
  private int origFontSize;

  void Start() {
    text = GetComponent<Text>();
    origFontSize = text.fontSize;

    //Subscribe to the change language event
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;

    //Run the method one first time
    OnChangeLanguage(languageManager);
  }

  public void increment() {
    count++;
    text.text = description + " " + count + "/" + limit;
    if (count == limit) {
      tutoHandler.nextTutorial(1);
    }
  }

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager)
  {
    description = LanguageManager.Instance.GetTextValue("Tutorial_GetCandies");
    text.text = description + " 0/" + limit;
    text.font = LangManager.lm.getFont();
    text.fontSize = (int)(origFontSize * LangManager.lm.getFontScale());
  }
}
