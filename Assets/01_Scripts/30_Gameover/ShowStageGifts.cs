using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowStageGifts : MonoBehaviour {
  public float showTerm = 0.3f;
  public float pitchUpBy = 0.1f;
  public float showBannerAfter = 0.3f;
  bool shown = false;

  void OnEnable() {
    if (shown) return;
    shown = true;
    StartCoroutine("showGifts");
  }

  IEnumerator showGifts() {
    int count = 0;
    foreach (Transform tr in transform) {
      yield return new WaitForSeconds(showTerm);
      tr.GetComponent<AudioSource>().pitch += pitchUpBy * count;
      count++;
      tr.Find("ClearText").GetComponent<Text>().text = "level " + count + "\nbonus !!";
      tr.gameObject.SetActive(true);
    }

    yield return new WaitForSeconds(showBannerAfter);

    transform.parent.parent.GetComponent<ScoreUpdate>().increaseStatus();
  }
}
