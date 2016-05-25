using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SmartLocalization;

public class RestorePurchasesButton : MenusBehavior {
  public LocalText resultText;
  public string successTextKey;
  public string failedTextKey;
  bool interactable = true;

  override public void activateSelf() {
    if (interactable) {
      BillingManager.bm.RestorePurchases((bool result) => {
        if (result) {
          resultText.key = successTextKey;
          resultText.reloadText();
          resultText.gameObject.SetActive(true);
          StartCoroutine(blinkForSecs(2, resultText));
        } else {
          resultText.key = failedTextKey;
          resultText.reloadText();
          resultText.gameObject.SetActive(true);
          StartCoroutine(blinkForSecs(2, resultText));
        }
      });
    }
  }
  
  IEnumerator blinkForSecs(float seconds, LocalText target) {
    interactable = false;
    float elapsed = 0;
    while (elapsed < seconds) {
      yield return new WaitForSeconds(0.4f);
      elapsed += Time.deltaTime + 0.4f;
      target.gameObject.SetActive(!target.gameObject.activeInHierarchy);
    }
    target.gameObject.SetActive(false);
    interactable = true;
  }
}
