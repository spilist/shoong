using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IdleHandler : MonoBehaviour {
  public GameObject touchToStart;
  public float blinkingSeconds = 0.6f;

  void Start () {
    StartCoroutine(BlinkText());
	}

  IEnumerator BlinkText() {
    while(true) {
      touchToStart.GetComponent<Text>().enabled = false;

      yield return new WaitForSeconds(1 - blinkingSeconds);

      touchToStart.GetComponent<Text>().enabled = true;

      yield return new WaitForSeconds(blinkingSeconds);
    }
  }

  void OnEnable() {
    StartCoroutine(BlinkText());
  }
}
