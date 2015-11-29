using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;

public class LangManager : MonoBehaviour {
  public static LangManager lm;
  public string lang;
  public bool autoLangDetection = true;
  public LocalFont[] localFonts;
  private Dictionary<string, LocalFont> fontDict;

  private LanguageManager languageManager;

  void Awake() {
    lm = this;
    fontDict = new Dictionary<string, LocalFont>();
    foreach (LocalFont localFont in localFonts) {
      fontDict[localFont.name] = localFont;
    }

    if (autoLangDetection) {
      lang = systemLanguageToCode(Application.systemLanguage);
    }
  }

  void Start () {
    languageManager = LanguageManager.Instance;
    languageManager.ChangeLanguage(lang);
  }

  public Font getFont() {
    return fontDict[lang].font;
  }

  public float getFontScale() {
    return fontDict[lang].fontScale;
  }

  string systemLanguageToCode(SystemLanguage sl) {
    switch (sl) {
      case SystemLanguage.Korean:
      return "ko";
      case SystemLanguage.English:
      return "en";
      case SystemLanguage.Japanese:
      return "ja";
      default:
      return "en";
    }
  }
}

[Serializable]
public struct LocalFont {
  public string name;
  public Font font;
  public float fontScale;
}
