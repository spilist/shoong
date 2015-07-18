using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboBar : MonoBehaviour {
  public Image inner;
  public Image outer;
  public Text[] comboRatio;
  public ParticleSystem comboGlow;
  public GameObject player;

  public float tintAmount = 0.02f;

  private int comboCount = 0;
  private Color trailTintColor;

  void Start () {
	}

  public void addCombo() {
    if (comboCount < 4) {
      comboCount++;
      comboGlow.emissionRate += 50;

      hideComboRatios();
      comboRatio[comboCount-1].enabled = true;
      comboRatio[comboCount-1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();

      trailTintColor = player.GetComponent<TrailRenderer>().material.GetColor("_TintColor");
      trailTintColor.a += tintAmount;
      player.GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailTintColor);

    }
    inner.fillAmount += 0.25f;
    outer.fillAmount = inner.fillAmount;
  }

  public void loseByShoot() {
    int fill = (int)(outer.fillAmount * 100);
    outer.fillAmount -= 0.125f;
    if (fill % 25 != 0) {
      inner.fillAmount -= 0.25f;
      hideComboRatios();
      if (comboCount > 0) {
        comboCount--;
        comboGlow.emissionRate -= 50;

        trailTintColor = player.GetComponent<TrailRenderer>().material.GetColor("_TintColor");
        trailTintColor.a -= tintAmount;
        player.GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailTintColor);

        if (comboCount != 0) {
          comboRatio[comboCount-1].enabled = true;
        }
      }
    }
  }

  public int getComboRatio() {
    return (comboCount + 1);
  }

  private void hideComboRatios() {
    for (int i = 0; i < comboRatio.Length; i++) {
      comboRatio[i].enabled = false;
    }
  }
}
