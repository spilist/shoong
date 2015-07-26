using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnstoppableComboBar : MonoBehaviour {
  public PlayerMover player;
  public float blinkingSeconds = 0.2f;

  private Image image;
  private int fullComboCount;

	void Start () {
    image = GetComponent<Image>();
    fullComboCount = player.max_unstoppable_combo;
	}

  public void addCombo() {
    image.fillAmount += 1f / fullComboCount;
  }

  public void startUnstoppable() {
    StartCoroutine("startDecrase");
  }

  IEnumerator startDecrase() {
    while(image.fillAmount > 0f) {
      if (image.fillAmount <= 1f / fullComboCount) {
        float blinking = 1;
        while(blinking > 0f) {
          yield return new WaitForSeconds(blinkingSeconds);
          image.enabled = !image.enabled;
          blinking -= blinkingSeconds;
        }
        image.enabled = true;
        image.fillAmount = 0;
      }
      else {
        yield return new WaitForSeconds(1);
        image.fillAmount -= 1f / fullComboCount;
      }
    }
  }
}
