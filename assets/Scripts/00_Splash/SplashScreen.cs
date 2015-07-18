using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {	
	float startTime;
	int delay = 2;
	int stage = 0;
	bool isSet = false;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float elapsedTime = Time.time - startTime;
		if (stage == 0) {
			if (isSet == false) {
				GetComponent<Image>().canvasRenderer.SetAlpha( 0.0f );
				GetComponent<Image>().CrossFadeAlpha(1.0f, 2.0f, false);
			}
			isSet = true;
			if (elapsedTime > delay) {
				startTime = Time.time;
				stage++;
				isSet = false;
			}
		} else if (stage == 1) {
			isSet = true;
			if (elapsedTime > delay) {
				startTime = Time.time;
				stage++;
				isSet = false;
			}

		} else if (stage == 2) {
			if (isSet == false){
				GetComponent<Image>().CrossFadeAlpha(0.0f, 2.0f, false);
			}
			isSet = true;
			if (elapsedTime > delay) {
				startTime = Time.time;
				stage++;
				isSet = false;
			}
		} else {
			Application.LoadLevel(1);
		}
	}
}
