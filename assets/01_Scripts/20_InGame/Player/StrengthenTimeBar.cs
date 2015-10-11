using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrengthenTimeBar : MonoBehaviour {
  public PlayerMover player;
  public StrengthenTimeBlinkingBar stb;

  private Image image;
  private int count = 0;
  private float decreaseAmount;

	void Start () {
    image = GetComponent<Image>();
    decreaseAmount = 1f / player.strengthen_during;
	}

  public void startStrengthen(int duration) {
    StopCoroutine("startDecrase");
    count = duration;
    stb.startStrengthen(duration);
    image.fillAmount = (duration - 1) * decreaseAmount;
    StartCoroutine("startDecrase");
  }

  IEnumerator startDecrase() {
    while(count > 0) {
      yield return new WaitForSeconds(1);
      image.fillAmount -= decreaseAmount;
      if (count > 1) stb.timeElapse();
      count--;
    }
    stb.stopStrengthen();
  }

  public void stop() {
    StopCoroutine("startDecrase");
    image.fillAmount = 0;
    stb.stopStrengthen();
  }
}
