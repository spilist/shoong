using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboBar : MonoBehaviour {
  public Image inner;
  public Text[] comboRatio;
	public ParticleSystem getEnergy;
	public ParticleSystem energyDestroy;
  public GameObject player;

  public float tintAmount = 0.02f;
	public float speedraiseamount;

  private int comboCount = 0;
	private int fill = 1;
  private Color trailTintColor;
	private Color comboBarTintColor;
	private IEnumerator coroutine;
	private Color energyDestroyColor;

	public float blinkingSeconds = 0.4f;
	public float moverspeed;

	void Start () {
		comboBarTintColor = inner.material.GetColor ("_TintColor");
		energyDestroy.emissionRate = 20;
    coroutine = BlinkCombobar ();
	}

	IEnumerator BlinkCombobar() {
		while (true) {

			comboBarTintColor.a = 0.3f;
			inner.material.SetColor ("_TintColor", comboBarTintColor);

			yield return new WaitForSeconds (0.8f - blinkingSeconds);

			comboBarTintColor.a = 1.0f;
			inner.material.SetColor ("_TintColor", comboBarTintColor);

			yield return new WaitForSeconds (blinkingSeconds);
		}
	}

  public void addCombo() {
    if (comboCount < 4) {
      comboCount++;
			getEnergy.emissionRate += 100;
			comboBarTintColor = inner.material.GetColor ("_TintColor");
				comboBarTintColor.a = 1.0f;
				inner.material.SetColor("_TintColor",comboBarTintColor);

      hideComboRatios();
      comboRatio[comboCount-1].enabled = true;
      comboRatio[comboCount-1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();

			energyDestroy.emissionRate += 10;
			moverspeed += speedraiseamount;

			//energyDestroyColor = energyDestroy.GetComponent<ParticleSystem>().startColor;
			//	energyDestroyColor.g -= 0.2f;
			//energyDestroy.GetComponent<ParticleSystem>().startColor= energyDestroyColor;

      //trailTintColor = player.GetComponent<TrailRenderer>().material.GetColor("_TintColor");
      //trailTintColor.a += tintAmount;
      //player.GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailTintColor);

    }
    inner.fillAmount += 0.25f;
		fill = 1;
		StopCoroutine(coroutine);
		comboBarTintColor.a = 1.0f;
		inner.material.SetColor("_TintColor",comboBarTintColor);
  }

  public void loseByShoot() {
    if (comboCount > 0) {
			if (fill == 1) {
				fill = 0;
				StartCoroutine(coroutine);
			} else {
				fill = 1;
				inner.fillAmount = 0f;
				hideComboRatios ();
				StopCoroutine(coroutine);
				comboBarTintColor.a = 1.0f;
				inner.material.SetColor("_TintColor",comboBarTintColor);
				if (comboCount > 0) {
					//comboCount--;
					comboCount = 0;
					getEnergy.emissionRate = 0;
					energyDestroy.emissionRate = 20;
					moverspeed = 45f;

					//energyDestroyColor = energyDestroy.GetComponent<ParticleSystem>().startColor;
					//energyDestroyColor.g = 0.8f;
					//energyDestroy.GetComponent<ParticleSystem>().startColor= energyDestroyColor;


					//trailTintColor = player.GetComponent<TrailRenderer> ().material.GetColor ("_TintColor");
					//trailTintColor.a = 0.14f;
					//player.GetComponent<TrailRenderer> ().material.SetColor ("_TintColor", trailTintColor);

					//if (comboCount != 0) {
					//comboRatio[comboCount-1].enabled = true;
					//}
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
