using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnstoppableComboBar : MonoBehaviour {
  public PlayerMover player;
  public UnstoppableBlinkingComboBar ubc;

  private Image image;
  private int fullComboCount;
  private int comboCount = 0;

	void Start () {
    image = GetComponent<Image>();
    fullComboCount = player.max_unstoppable_combo;
	}

  public void addCombo() {
    ubc.addCombo(comboCount);
    comboCount++;
    if (comboCount > 1) image.fillAmount += 1f / fullComboCount;
  }

  public void startUnstoppable() {
    StartCoroutine("startDecrase");
    ubc.startUnstoppable();
  }

  IEnumerator startDecrase() {
    while(comboCount > 0) {
      yield return new WaitForSeconds(1);
      image.fillAmount -= 1f / fullComboCount;
      if (comboCount > 1) ubc.timeElapse();
      comboCount--;
    }
    ubc.stopUnstoppable();
  }
}
