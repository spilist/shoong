﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoManager : MonoBehaviour {
  public Text FPS;
  float fpsNum;

	void Update () {
    FPS.text = (1.0f / Time.smoothDeltaTime).ToString("0.00");
	}
}