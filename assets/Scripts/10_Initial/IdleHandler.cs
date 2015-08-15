using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IdleHandler : MonoBehaviour {
  public GameObject touchToStart;
  public float blinkingSeconds = 0.6f;
  public Text cubesYouHave;
  public Text goldenCubesYouHave;

  void Start () {
    StartCoroutine(BlinkText());
    cubesYouHave.text = ((int)GameController.control.cubes["now"]).ToString();
    goldenCubesYouHave.text = ((int)GameController.control.goldenCubes["now"]).ToString();
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
