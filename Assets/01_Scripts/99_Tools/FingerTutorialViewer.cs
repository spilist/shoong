using UnityEngine;
using System.Collections;
using System;

public class FingerTutorialViewer : MonoBehaviour {
  public GameObject helper;
  bool started = false;
  DateTime startDisableTime;

	void Start () {
	  if (DataManager.dm.getBool("FirstPlay")) {
      helper.SetActive(true);
    }
	}

  public void disableViewer() {
    if (!started) {
      started = true;
      startDisableTime = DateTime.Now;
    } else {
      if (DateTime.Now > startDisableTime.AddSeconds(2)) {
        helper.SetActive(false);
      }
    }
  }
}
