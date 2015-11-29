using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FreeGift : MonoBehaviour {
  public GameObject overlay;
  public GameObject giftBox;
  public Text goldCubeEarned;
  public int originalSize = 14;
  public int shrinkSize = 8;
  public float openAfter = 0.5f;
  public float totalSeconds = 1;
  public float interval = 0.1f;
  public float hideAfter = 2;
  public GameObject openingParticle;

  private int reward;

  private FreeRewardBannerButton fgb;

  public void run(FreeRewardBannerButton fgb, int reward) {
    gameObject.SetActive(true);
    overlay.SetActive(true);
    this.fgb = fgb;
    this.reward = reward;

    StartCoroutine("openBox");
  }

  IEnumerator openBox() {
    yield return new WaitForSeconds(openAfter);

    Vector3 originalScale = originalSize * Vector3.one;
    Vector3 shrinkScale = shrinkSize * Vector3.one;

    float duration = totalSeconds;
    while (duration > 0) {
      giftBox.transform.localScale = shrinkScale;
      yield return new WaitForSeconds(interval);
      giftBox.transform.localScale = originalScale;
      yield return new WaitForSeconds(interval);

      duration -= interval * 2;
    }

    giftBox.SetActive(false);
    openingParticle.SetActive(true);

    goldCubeEarned.text = reward.ToString();
    goldCubeEarned.transform.parent.gameObject.SetActive(true);

    yield return new WaitForSeconds(hideAfter);

    gameObject.SetActive(false);
  }

  void OnDisable() {
    overlay.SetActive(false);
    fgb.endFreeGift();
  }
}
