using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class GoogleAuthButton : OnOffButton {
  public Text text;
  public Color activeColor;
  public Color inactiveColor;
  private string signInString;
  private string signOutString;
  private Image image;

  override public void initializeRest() {
    image = GetComponent<Image>();
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;

    //Run the method one first time
    OnChangeLanguage(languageManager);
  }

  override public void activateSelf() {
    if (clicked) {
      SocialPlatformManager.spm.authenticate((bool _success)=>{
        if (_success)
        {
          base.activateSelf();
        }
        else
        {
        }
      });
    } else {
      SocialPlatformManager.spm.signout();
      base.activateSelf();
    }
  }

  override public void applyStatus() {
    if (clicked) {
      text.text = signInString;
      image.color = activeColor;
    } else {
      image.color = inactiveColor;
      text.text = signOutString;
    }
  }

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager) {
    signInString = LanguageManager.Instance.GetTextValue("Setting_Google_SignIn");
    signOutString = LanguageManager.Instance.GetTextValue("Setting_Google_SignOut");
  }
}
