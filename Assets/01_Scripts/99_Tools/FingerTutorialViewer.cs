using UnityEngine;
using System.Collections;
using System;

public class FingerTutorialViewer : MonoBehaviour {
  public GameObject helper;
  DateTime startDisableTime;

	void Start () {
	  if (DataManager.dm.getBool("FirstPlay")) {
      helper.SetActive(true);
    }
	}

  public void disableViewer() {
    Invoke("disable", 2);
  }

  void disable() {
    helper.SetActive(false);
  }
}
