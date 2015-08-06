using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnstoppableComboBar : MonoBehaviour {
  public PlayerMover player;
  public UnstoppableBlinkingComboBar ubc;

  private Image image;
  private int during;
  private int count = 0;

	void Start () {
    image = GetComponent<Image>();
    during = (int) player.unstoppable_during;
	}

  public void startUnstoppable() {
    count = during;
    image.fillAmount = 1f - 1f / during;
    ubc.startUnstoppable();
    StartCoroutine("startDecrase");
  }

  IEnumerator startDecrase() {
    while(count > 0) {
      yield return new WaitForSeconds(1);
      image.fillAmount -= 1f / during;
      if (count > 1) ubc.timeElapse();
      count--;
    }
    ubc.stopUnstoppable();
  }
}
