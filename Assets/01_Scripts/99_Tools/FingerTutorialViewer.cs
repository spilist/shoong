using UnityEngine;
using System.Collections;
using System;

public class FingerTutorialViewer : MonoBehaviour {
  public GameObject helper;
  bool started = false;
  DateTime startDisableTime;
	// Use this for initialization
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
      if (DateTime.Now.AddSeconds(2) > startDisableTime) {
        helper.SetActive(false);
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
