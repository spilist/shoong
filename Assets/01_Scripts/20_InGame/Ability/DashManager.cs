using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DashManager : MonoBehaviour {
  public static DashManager dm;

  public DashButton dash;
  public AudioSource dashSound;
  public Color dimmedEffectColor;
  public Color enabledEffectColor;

  public AudioSource getStackSound;
  public UIEffect dashEffect;
  public ParticleSystem playerDashEffect;
  public float pitchStart = 0.05f;
  public float pitchIncrease = 0.6f;

  public float duration = 0.3f;
  public int numRotateWhenSmash = 2;
  public float speedup = 300;

  public float maxEnlargeSize = 1.6f;
  public int maxStack = 5;
  private int currentStack = 0;
  private bool withSound = true;

  public GameObject afterImagePrefab;
  private List<GameObject> afterImagePool;
  public int afterImageCount = 10;
  public float generatePer = 0.1f;
  public float afterImageDuration = 1;
  public Color originalColor;

  void Awake() {
    dm = this;
    afterImagePool = new List<GameObject>();
    for (int i = 0; i < afterImageCount; ++i) {
      GameObject obj = (GameObject) Instantiate(afterImagePrefab);
      obj.SetActive(false);
      afterImagePool.Add(obj);
    }
  }

  public void getLarger() {
    if (currentStack < maxStack) {
      currentStack++;
      Player.pl.scaleChange(currentStack * (maxEnlargeSize - 1) / maxStack);

      if (currentStack == maxStack) {
        dashEffect.gameObject.SetActive(true);
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
    StopCoroutine("afterImage");
  }

  public bool available() {
    return currentStack == maxStack;
  }

  public void smash() {
    Player.pl.dash();
    playerDashEffect.Play();
    dashSound.Play();
    dashEffect.diminish();
    StartCoroutine("afterImage");
    dash.GetComponent<Image>().color = dimmedEffectColor;
    dash.transform.Find("Text").GetComponent<Text>().color = dimmedEffectColor;
  }

  public void enableSmash() {
    dash.GetComponent<Image>().color = enabledEffectColor;
    dash.transform.Find("Text").GetComponent<Text>().color = enabledEffectColor;
  }

  IEnumerator afterImage() {
    while (true) {
      GameObject afterImage = getAfterImage();
      afterImage.transform.position = Player.pl.transform.position;
      afterImage.transform.rotation = Player.pl.transform.rotation;
      afterImage.SetActive(true);

      afterImage.GetComponent<PowerBoostAfterImageMover>().run(afterImageDuration, originalColor, Player.pl.transform.parent.localScale.x * Player.pl.transform.localScale.x);

      yield return new WaitForSeconds(generatePer);
    }
  }

  GameObject getAfterImage() {
    for (int i = 0; i < afterImagePool.Count; i++) {
      if (!afterImagePool[i].activeInHierarchy) {
        return afterImagePool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(afterImagePrefab);
    afterImagePool.Add(obj);
    return obj;
  }
}
