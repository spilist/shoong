using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboBar : MonoBehaviour {
  public Image inner;
  public Image innerBlinking;
	public ParticleSystem getEnergy;

  public float tintAmount = 0.02f;

  private int comboCount = 0;
  private Color comboBarTintColor;
	private Color comboBarTintColor_empty;
  private Color comboBarTintColor_full;
  public float comboBarTintColorEmptyAlpha = 0.3f;

  public float loseAfter = 1.9f;
  public float showDurationStart = 0.45f;
	public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.25f;
  public float emptyDurationDecrease = 0.05f;

  public int emissionRate = 100;
  public Color[] comboBarColors;

	void OnEnable () {
		comboBarTintColor = inner.material.GetColor ("_TintColor");
		comboBarTintColor_empty = new Color(comboBarTintColor.r, comboBarTintColor.g, comboBarTintColor.b, comboBarTintColorEmptyAlpha);
    comboBarTintColor_full = new Color(comboBarTintColor.r, comboBarTintColor.g, comboBarTintColor.b, 1);
	}

  IEnumerator loseByTime() {
    float duration = loseAfter;
    float showDuring = showDurationStart;
    float emptyDuring = emptyDurationStart;
    while (duration > 0) {
      inner.material.SetColor ("_TintColor", comboBarTintColor_full);
      yield return new WaitForSeconds (showDuring);

      inner.material.SetColor ("_TintColor", comboBarTintColor_empty);
      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring>1f) showDuring -= showDurationDecrease;
			if(emptyDuring>0.5f) emptyDuring -= emptyDurationDecrease;
    }

    comboCount = 0;
    inner.fillAmount = 0;
    getEnergy.emissionRate = 0;
  }

  public void addCombo() {
    if (comboCount < 4) {
      comboCount++;
      inner.fillAmount += 0.25f;
      inner.material.SetColor("_TintColor",comboBarTintColor_full);
      getEnergy.emissionRate += emissionRate;

      if (comboCount == 4) {
        // Player.pl.showEffect("Charged");
      }
    }
    StopCoroutine("loseByTime");
    if (gameObject.activeSelf) StartCoroutine("loseByTime");
  }

  public int getComboRatio() {
    return (comboCount + 1);
  }

  void OnDisable() {
    inner.material.SetColor ("_TintColor", comboBarTintColor_empty);
  }
}
