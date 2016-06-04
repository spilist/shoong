using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashManager : MonoBehaviour {
  public static DashManager dm;

  public DashButton dash;
  public Color dimmedEffectColor;
  public Color enabledEffectColor;

  public AudioSource getStackSound;
  public GameObject dashEffect;
  public float pitchStart = 0.05f;
  public float pitchIncrease = 0.6f;

  public float duration = 0.3f;
  public float speedup = 300;

  public float maxEnlargeSize = 1.6f;
  public int maxStack = 5;
  private int currentStack = 0;
  private bool withSound = true;

  void Awake() {
    dm = this;
  }

  public void getLarger() {
    if (currentStack < maxStack) {
      currentStack++;
      Player.pl.scaleChange(currentStack * (maxEnlargeSize - 1) / maxStack);

      if (currentStack == maxStack) {
        dashEffect.SetActive(true);
        enableSmash();
        withSound = false;
      }
    }

    if (withSound) {
      getStackSound.pitch = pitchStart + currentStack * pitchIncrease;
      getStackSound.Play();
    }

    withSound = true;
  }

  public void resetStep() {
    currentStack = 0;
  }

  public bool available() {
    return currentStack == maxStack;
  }

  public void smash() {
    Player.pl.dash();
    dashEffect.SetActive(false);
    dash.GetComponent<Image>().color = dimmedEffectColor;
    dash.transform.Find("Text").GetComponent<Text>().color = dimmedEffectColor;
  }

  public void enableSmash() {
    dash.GetComponent<Image>().color = enabledEffectColor;
    dash.transform.Find("Text").GetComponent<Text>().color = enabledEffectColor;
  }
}
