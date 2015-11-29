using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoModeIndicator : MonoBehaviour {
	public AutoBoosterButton abb;
  public GameObject icon;

  public float startScale = 0.1f;
  public float stayScale = 1;
  public float smallScale = 0.6f;
  public float largeScale = 1.2f;

  public float changeTime = 0.1f;
  public float stayTime = 2f;

  private float scale;
  private int status = 0;
  private float stayCount = 0;

  void OnEnable() {
    // icon.transform.Find("Count").GetComponent<Text>().text = "-" + abb.price;
    if (abb.isOn()) {
      // if (!abb.decrementGold()) icon.SetActive(false);
      scale = startScale;
      status++;
    }
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
        if (stayCount < stayTime) stayCount += Time.deltaTime;
        else status++;
      } else if (status == 5) {
        changeScale(largeScale, largeScale - stayScale);
      } else if (status == 6) {
        changeScale(startScale, startScale - largeScale);
      } else if (status == 7) {
        gameObject.SetActive(false);
      }

      transform.localScale = scale * Vector3.one;
    }
  }

  void OnDisable() {
    status = 0;
    stayCount = 0;
  }

  void changeScale(float targetScale, float difference) {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (scale == targetScale) status++;
  }
}
