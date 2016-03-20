using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageGift : MonoBehaviour {
  public float startScale = 0.1f;
  public float stayScale = 1;
  public float smallScale = 0.6f;
  public float largeScale = 1.2f;
  public float changeTime = 0.1f;

  private float scale;
  private int status = 0;
  private float stayCount;
  bool shown = false;

  void OnEnable() {
    if (shown) return;

    shown = true;
    GetComponent<AudioSource>().Play();
    transform.localScale = startScale * Vector3.one;
    scale = startScale;
    status++;
  }

	void Update() {
    if (status > 0) {
      if (status == 1) {
        changeScale(largeScale, largeScale - startScale);
      } else if (status == 2) {
        changeScale(smallScale, smallScale - largeScale);
      } else if (status == 3) {
        changeScale(stayScale, stayScale - smallScale);
      } else if (status == 4) {
        status++;
      }

      transform.localScale = scale * Vector3.one;
    }
  }

  void changeScale(float targetScale, float difference) {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (scale == targetScale) status++;
  }
}
