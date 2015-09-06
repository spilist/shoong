using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	private int time = 0;

	void OnEnable() {
		StartCoroutine("startElapse");
	}

	IEnumerator startElapse() {
		while(true) {
			yield return new WaitForSeconds(1);
			time++;
			// GetComponent<Text>().text = time.ToString();
		}
	}

	public int getTime() {
		return (int) time;
	}

	void OnDisable() {
		StopCoroutine("startElapse");
	}
}
