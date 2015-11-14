using UnityEngine;
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
				transform.parent.gameObject.SetActive(false);
				if (DataManager.dm.getBool("TutorialDone")) {
				  Application.LoadLevelAsync("2_BeforeMainScene");
				} else {
				  Application.LoadLevelAsync("_Tutorial");
				}
			}
		}
	}
}
