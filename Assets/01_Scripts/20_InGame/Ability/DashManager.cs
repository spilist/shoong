using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DashManager : MonoBehaviour {
  public static DashManager dm;

  public DashButton dash;
  public AudioSource dashSound;
  public ParticleSystem smashOnParticle;
  public ParticleSystem supersmashOnParticle;
  public ParticleSystem smashCharged;
  public ParticleSystem supersmashCharged;

  public Color dimmedEffectColor;
  public Color enabledEffectColor;

  public AudioSource getStackSound;
  public UIEffect smashOnEffect;
  public UIEffect supersmashOnEffect;
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
  private bool dashAvailable = false;

  void Awake() {
    dm = this;
  }

  public void gameStart() {
    afterImagePrefab.GetComponent<MeshFilter>().mesh = Player.pl.GetComponent<MeshFilter>().sharedMesh;
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
    return ((currentStack == maxStack && dashAvailable) && (!Player.pl.uncontrollable()));
  }

  public void smash(bool decreseCooldown) {
    Player.pl.dash();
    playerDashEffect.Play();
    dashSound.Play();
    StartCoroutine("afterImage");

    dashAvailable = false;

    dash.GetComponent<Image>().color = dimmedEffectColor;
    dash.transform.Find("SmashText").GetComponent<Text>().color = dimmedEffectColor;

    if (decreseCooldown) {
      if (SkillManager.sm.skillAvailable()) {
        supersmashOnEffect.diminish();
        supersmashCharged.Stop();
      } else {
        smashOnEffect.diminish();
        smashCharged.Stop();
      }

      SkillManager.sm.activateWithDash();
    }
  }

  public void enableSmash() {
    dashAvailable = true;

    dash.GetComponent<Image>().color = enabledEffectColor;
    dash.transform.Find("SmashText").GetComponent<Text>().color = enabledEffectColor;

    if (SkillManager.sm.skillAvailable()) {
      supersmashOnEffect.gameObject.SetActive(true);
      supersmashOnParticle.Play();
      supersmashCharged.Play();
    } else {
      smashOnEffect.gameObject.SetActive(true);
      smashOnParticle.Play();
      smashCharged.Play();
    }
  }

  public void stopEffects() {
    supersmashCharged.Stop();
    smashCharged.Stop();
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
