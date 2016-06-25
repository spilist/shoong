using UnityEngine;
using System.Collections;

public class PopOnEnable : MonoBehaviour {
  public float scaleStandard;
  public float stayBeforeStart = 0;
  public float startScale = 0;
  public float stayScale = 1;
  public float smallScale = 0.6f;
  public float largeScale = 1.2f;

  public float changeTime = 0.1f;

  private float scale;
  private int status = 0;
  private float stayCount;

  void OnEnable() {
    transform.localScale = startScale * scaleStandard * Vector3.one;
    scale = startScale * scaleStandard;
    status = 1;
  }

  void Update() {
    if (status > 0) {
      if (status == 1) {
        if (stayCount < stayBeforeStart) stayCount += Time.deltaTime;
        else changeScale(largeScale, largeScale - startScale);
      } else if (status == 2) {
        changeScale(smallScale, smallScale - largeScale);
      } else if (status == 3) {
        changeScale(stayScale, stayScale - smallScale);
      } else if (status == 4) {
        status = 0;
      }

      transform.localScale = scale * scaleStandard * Vector3.one;
    }
  }

  void changeScale(float targetScale, float difference) {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (scale == targetScale) status++;
  }
}
