using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrengthenTimeBar : MonoBehaviour {
  public PlayerMover player;
  public StrengthenTimeBlinkingBar stb;

  private Image image;
  private int during;
  private int count = 0;

	void Start () {
    image = GetComponent<Image>();
    during = (int) player.strengthen_during;
	}

  public void startStrengthen() {
    count = during;
    image.fillAmount = 1f - 1f / during;
    stb.startStrengthen();
    StartCoroutine("startDecrase");
  }

  IEnumerator startDecrase() {
    while(count > 0) {
      yield return new WaitForSeconds(1);
      image.fillAmount -= 1f / during;
      if (count > 1) stb.timeElapse();
      count--;
    }
    stb.stopStrengthen();
  }

  public void rebounded(int rebonudDuring) {
    StopCoroutine("startDecrase");
    count = rebonudDuring;
    stb.setCount(rebonudDuring - 1);
    image.fillAmount = 1f / during;
    StartCoroutine("startDecrase");
  }
}
