using UnityEngine;
using System.Collections;
using System;

public class FingerTutorialViewer : MonoBehaviour {
  public PauseButton pause;
  public float activateAfter = 0.6f;

  public GameObject helper;
  public GameObject topLeftGuide;
  public GameObject bottomRightGuide;

  bool started = false;
  DateTime startDisableTime;

  int guideCount = 0;

	void Start () {
	}

  public void showViewer() {
    if (DataManager.dm.getBool("FirstPlay")) {
      StartCoroutine(startGuide());
    }
  }

  IEnumerator startGuide() {
    yield return new WaitForSeconds(activateAfter);

    pause.activatePause();
    topLeftGuide.SetActive(true);
  }

  public void disableViewer() {
    if (!DataManager.dm.getBool("FirstPlay")) {
      return;
    }

    guideCount++;

    if (guideCount == 1) {
      topLeftGuide.SetActive(false);
      bottomRightGuide.SetActive(true);
    } else if (guideCount == 2) {
      bottomRightGuide.SetActive(false);
      helper.SetActive(true);
      pause.activateResume();
    } else if (!started) {
      started = true;
      startDisableTime = DateTime.Now;
    } else {
      if (DateTime.Now > startDisableTime.AddSeconds(2)) {
        helper.SetActive(false);
      }
    }
  }
}
