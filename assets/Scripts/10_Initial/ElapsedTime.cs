using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	private bool timeChanging = false;
	private float time = 0.0f;

	void Start () {
	}

	void Update () {
		if (timeChanging) {
			time += Time.deltaTime;
			GetComponent<Text>().text = time.ToString ("0");
		}

	}

	public void startTime() {
		timeChanging = true;
	}

	public void stopTime() {
		timeChanging = false;
	}
}
