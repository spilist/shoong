using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoManager : MonoBehaviour {
  public Text FPS;
  float fpsNum;

	void Update () {
    //FPS.text = (1.0f / Time.smoothDeltaTime).ToString("0.00");
    //FPS.text = AudioManager.am.main.currentAudioSource.pitch + "";
    FPS.text = ((int)(Player.pl.lrSpeedModifier * 100)) / 100.0 + "";
	}
}
