using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class DreamingText : MonoBehaviour {
  public float speed = 0.05f;
  private Text text;
  private string description;
  private int origFontSize;

  void Start () {
    text = GetComponent<Text>();
    origFontSize = text.fontSize;

    //Subscribe to the change language event
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;

    //Run the method one first time
    OnChangeLanguage(languageManager);

    StartCoroutine(AnimateText());
  }

  IEnumerator AnimateText(){
    // GetComponent<AudioSource>().Play();
    for (int i = 0; i < description.Length+1; i++) {
      text.text = description.Substring(0, i);
      yield return new WaitForSeconds(speed);
    }
    // GetComponent<AudioSource>().Stop();
  }

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager)
  {
    description = LanguageManager.Instance.GetTextValue("Tutorial_ChildDreaming");
    text.font = LangManager.lm.getFont();
    text.fontSize = (int)(origFontSize * LangManager.lm.getFontScale());
  }
}
