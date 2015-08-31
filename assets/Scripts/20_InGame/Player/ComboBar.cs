using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboBar : MonoBehaviour {
  public Image inner;
  public Image innerBlinking;
	public ParticleSystem getEnergy;
  public GameObject player;

  public float tintAmount = 0.02f;
	public float speedraiseamount;

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

  public float originalSpeed = 45;
  public int emissionRate = 100;
  public Color[] comboBarColors;

  private float moverspeed;

	void OnEnable () {
		comboBarTintColor = inner.material.GetColor ("_TintColor");
		comboBarTintColor_empty = new Color(comboBarTintColor.r, comboBarTintColor.g, comboBarTintColor.b, comboBarTintColorEmptyAlpha);
    comboBarTintColor_full = new Color(comboBarTintColor.r, comboBarTintColor.g, comboBarTintColor.b, 1);

    moverspeed = originalSpeed;
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
    moverspeed = originalSpeed;
    changeTrail();
  }

  public void addCombo() {
    if (comboCount < 4) {
      comboCount++;
      inner.fillAmount += 0.25f;
      inner.material.SetColor("_TintColor",comboBarTintColor_full);
      getEnergy.emissionRate += emissionRate;
      moverspeed += speedraiseamount;
      changeTrail();
    }
    StopCoroutine("loseByTime");
    StartCoroutine("loseByTime");
  }

  public int getComboRatio() {
    return (comboCount + 1);
  }

  public float getSpeed() {
    return moverspeed;
  }

  void changeTrail() {
    TrailRenderer trail = player.GetComponent<TrailRenderer>();
    trail.startWidth = 2 + comboCount;
    trail.endWidth = 1 + comboCount * 0.5f;
  }
}
