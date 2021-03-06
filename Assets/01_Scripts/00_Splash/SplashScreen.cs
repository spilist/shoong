﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
	public float splashDuring = 3f;
	private float count;
	private bool loaded = false;

	void Update () {
		if (count < splashDuring) count += Time.deltaTime;
		else {
			if (!loaded) {
				loaded = true;
				if (DataManager.dm.isFirstPlay()) {
          Application.LoadLevel("1_Opening");
        } else {
          Application.LoadLevel("2_BeforeMainScene");
				}
      }
		}
	}
}
