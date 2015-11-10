using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;

public class LangManager : MonoBehaviour {
  public static LangManager lm;
  public LocalFont[] localFonts;

  private LanguageManager languageManager;

  void Awake() {
    lm = this;
  }

  void Start () {

    string lang = DataManager.dm.getString("Language");
    lang = "ko";

    languageManager = LanguageManager.Instance;
    changeLanguage(lang);
  }

  public void changeLanguage(string lang) {
    DataManager.dm.setString("Language", lang);
    languageManager.ChangeLanguage(lang);
  }
}

[Serializable]
public struct LocalFont {
  public string name;
  public Font font;
  public float fontScale;
}
