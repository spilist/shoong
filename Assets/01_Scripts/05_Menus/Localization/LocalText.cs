using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class LocalText : MonoBehaviour {
  public string key;
  Text textObject;
  private int origFontSize;

	void Start () {
    textObject = this.GetComponent<Text>();
    origFontSize = textObject.fontSize;

    //Subscribe to the change language event
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;

    //Run the method one first time
    OnChangeLanguage(languageManager);
	}

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager)
  {
    if (key != "") {
      textObject.text = LanguageManager.Instance.GetTextValue(key);
    }

    textObject.font = LangManager.lm.getFont();
    textObject.fontSize = (int)(origFontSize * LangManager.lm.getFontScale());
  }
}
