using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
	public float splashDuring = 3f;
	private CanvasRenderer cr;
	private bool alphaIncreasing = true;

	void Start () {
		cr = GetComponent<Image>().canvasRenderer;
		cr.SetAlpha( 0.0f );
	}

	void Update () {
		if (alphaIncreasing) {
			cr.SetAlpha(Mathf.MoveTowards(cr.GetAlpha(), 1, Time.deltaTime * 2 / splashDuring));
			if (cr.GetAlpha() == 1) alphaIncreasing = false;
		} else {
			cr.SetAlpha(Mathf.MoveTowards(cr.GetAlpha(), 0, Time.deltaTime * 2 / splashDuring));
			if (cr.GetAlpha() == 0) Application.LoadLevel("2_BeforeMainScene");
		}
	}
}
