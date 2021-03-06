﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SuperheatManager : MonoBehaviour {
  public static SuperheatManager sm;
  public bool forceSuperheat;
  public SuperheatMover superheatMover;
  // public PartsToBeCollected ptb;
  public PowerBoostBackground powerBoostBackground;
  public RectTransform superImage;
  public RectTransform heatImage;
  // public Text touchIt;
  // public RectTransform touchItEffect;
  public GameObject afterImagePrefab;
  private List<GameObject> afterImagePool;
  public int afterImageCount = 50;

  // public int touchItEffectOriginalSize = 500;
  // public int touchItEffectShrinkedSize = 350;
  // private float touchItShrinkDuration;
  // private float touchItSize;

  public int startSuperPos = -895;
  public int slowSuperPos = 5;
  public int fastSuperPos = 45;
  public int endSuperPos = 945;

  public int startHeatPos = 945;
  public int slowHeatPos = 45;
  public int fastHeatPos = 5;
  public int endHeatPos = -895;

  private int transformStatus = 0;
  public float superheatFastDuration = 0.1f;
  public float superheatSlowDuration = 0.8f;
  private float superXPos;
  private float heatXPos;

  public float shakeDuration = 0.5f;
  public float shakeAmount = 3f;

  private bool isTransforming = false;
  public float boostDuration = 8;
  private float originalBoostDuration;

  public float baseSpeed = 200;
  public float boosterSpeedUpAmount = 250;
  public float maxBoosterSpeed = 400;
  private bool superheatRunning = false;

  private int superHeatCount = 0;

  // public float guageAlphaTurnedOn = 0.6f;
  // public float guageAlphaUp = 1;
  // public float guageAlphaDown = 0.2f;
  // public float guageAlphaChangeDuration = 1;
  // private Color guageColor;
  // private bool guageAlphaGoingUp = true;
  // private float guageAlpha;
  // private bool guageTurnedOn = false;

  // public float iconAlphaStayDuration = 2;
  // public float iconAlphaStayCount;

  // public float guageScaleUp = 4;
  // public Image guageIcon;
  // public GameObject bonusGuageIcon;
  public Skill_Superheat skill;
  // public Image guage;
  // public Image bonusGuage;
  public Image guageShell;
  // public GameObject superheatBonusCollider;
  // public int maxNumTouch = 10;
  // private float targetBonusGuage;
  // public float bonusGuageSpeedStandard = 400;

  public float maxGuage = 1000f;
  private float originalMaxGuage;
  public float guagePerCube = 0.5f;
  public float guageSpeedStandard = 200;
  private float targetGuage = 1;
  public int showHeatWhenLargerThan = 100;
  private bool activated = false;

	void Awake() {
    sm = this;
    originalMaxGuage = maxGuage;
    originalBoostDuration = boostDuration;
  }

  void Start() {
    // guageColor = guage.color;
    // guageAlpha = guageAlphaDown;
    // touchItShrinkDuration = superheatSlowDuration / maxNumTouch;

    afterImagePool = new List<GameObject>();
    for (int i = 0; i < afterImageCount; ++i) {
      GameObject obj = (GameObject) Instantiate(afterImagePrefab);
      obj.SetActive(false);
      afterImagePool.Add(obj);
    }
  }

  public void startGame() {
    Mesh mesh = Player.pl.GetComponent<MeshFilter>().sharedMesh;
    superheatMover.GetComponent<MeshFilter>().sharedMesh = mesh;

    for (int i = 0; i < afterImagePool.Count; i++) {
      afterImagePool[i].GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    if (forceSuperheat) {
      targetGuage = 0;
    }
  }

  void Update() {
    if (transformStatus > 0) {
      if (transformStatus == 1) {
        superXPos = Mathf.MoveTowards(superXPos, slowSuperPos, Time.deltaTime * Mathf.Abs(startSuperPos - slowSuperPos) / superheatFastDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, slowHeatPos, Time.deltaTime * Mathf.Abs(startHeatPos - slowHeatPos) / superheatFastDuration);
        if (superXPos == slowSuperPos) {
          transformStatus++;
          // superheatBonusCollider.SetActive(true);
          // touchIt.gameObject.SetActive(true);
          // touchItEffect.gameObject.SetActive(true);
          // touchItSize = touchItEffectOriginalSize;
        }
      } else if (transformStatus == 2) {
        superXPos = Mathf.MoveTowards(superXPos, fastSuperPos, Time.deltaTime * Mathf.Abs(fastSuperPos - slowSuperPos) / superheatSlowDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, fastHeatPos, Time.deltaTime * Mathf.Abs(fastHeatPos - slowHeatPos) / superheatSlowDuration);

        // touchItSize = Mathf.MoveTowards(touchItSize, touchItEffectShrinkedSize, Time.deltaTime * (touchItEffectOriginalSize - touchItEffectShrinkedSize) / touchItShrinkDuration);
        // touchItEffect.sizeDelta = touchItSize * Vector2.one;

        // if (touchItSize == touchItEffectShrinkedSize) {
          // touchItSize = touchItEffectOriginalSize;
        // }

        if (superXPos == fastSuperPos) {
          transformStatus++;
          // superheatBonusCollider.SetActive(false);
          // touchIt.gameObject.SetActive(false);
          // touchItEffect.gameObject.SetActive(false);
          // touchItEffect.sizeDelta = touchItEffectOriginalSize * Vector2.one;
        }
      } else if (transformStatus == 3) {
        superXPos = Mathf.MoveTowards(superXPos, endSuperPos, Time.deltaTime * Mathf.Abs(fastSuperPos - endSuperPos) / superheatFastDuration);
        heatXPos = Mathf.MoveTowards(heatXPos, endHeatPos, Time.deltaTime * Mathf.Abs(fastHeatPos - endHeatPos) / superheatFastDuration);
        if (superXPos == endSuperPos) transformStatus = 0;
      }
      superImage.anchoredPosition = new Vector2(superXPos, 0);
      heatImage.anchoredPosition = new Vector2(heatXPos, 0);
    }

    // if (canGetBonus()) {
    //   bonusGuage.fillAmount = Mathf.MoveTowards(bonusGuage.fillAmount, targetBonusGuage, Time.deltaTime / (maxGuage / bonusGuageSpeedStandard));
    //   if (bonusGuage.fillAmount == 1) bonusGuageIcon.SetActive(true);
    // }

    if (superheatRunning) {
      // if (bonusGuage.fillAmount > 0) {
      //   bonusGuage.fillAmount = Mathf.MoveTowards(bonusGuage.fillAmount, 0, Time.deltaTime / boostDuration);
      // } else {
      //   bonusGuageIcon.SetActive(false);
      //   guage.fillAmount = Mathf.MoveTowards(guage.fillAmount, 0, Time.deltaTime / boostDuration);
      // }
      guageShell.fillAmount = Mathf.MoveTowards(guageShell.fillAmount, 1, Time.deltaTime / boostDuration);
      if (guageShell.fillAmount == 1) stopSuperheat();

    } else {
      if (isTransforming) return;
      if (activated) return;
      guageShell.fillAmount = Mathf.MoveTowards(guageShell.fillAmount, targetGuage, Time.deltaTime / (maxGuage / guageSpeedStandard));

      if (guageShell.fillAmount == 0) {
        // startSuperheat();
        activated = true;
        skill.getReady();
        return;
      }

      // if (guageTurnedOn) {
      //   if (iconAlphaStayCount > 0) {
      //     iconAlphaStayCount -= Time.deltaTime;
      //   } else {
      //     guageTurnedOn = false;
      //     guageColor.a = guageAlphaDown;
      //     guage.color = guageColor;
      //     guageIcon.color = guageColor;
      //     guageAlphaGoingUp = true;
      //     guageIcon.transform.Find("Particle").gameObject.SetActive(false);
      //   }
      // } else {
      //   if (guageAlphaGoingUp) {
      //     guageAlpha = Mathf.MoveTowards(guageAlpha, guageAlphaUp, Time.deltaTime * (guageAlphaUp - guageAlphaDown) / guageAlphaChangeDuration);
      //     guageColor.a = guageAlpha;
      //     guage.color = guageColor;
      //     guageIcon.color = guageColor;
      //     if (guageAlpha == guageAlphaUp) guageAlphaGoingUp = false;
      //   } else {
      //     guageAlpha = Mathf.MoveTowards(guageAlpha, guageAlphaDown, Time.deltaTime * (guageAlphaUp - guageAlphaDown) / guageAlphaChangeDuration);
      //     guageColor.a = guageAlpha;
      //     guage.color = guageColor;
      //     guageIcon.color = guageColor;
      //     if (guageAlpha == guageAlphaDown) guageAlphaGoingUp = true;
      //   }
      // }
    }
  }

  public void addGuageWithEffect(float val) {
    if (superheatRunning || isTransforming) return;
    if (val == 0) return;

    // guageTurnedOn = true;
    // guageColor.a = 1;
    // guage.color = guageColor;
    // guageIcon.color = guageColor;
    // guageIcon.transform.Find("Particle").gameObject.SetActive(true);
    // iconAlphaStayCount = iconAlphaStayDuration;

    addGuage(val);
    if (val >= showHeatWhenLargerThan) {
      // Player.pl.showEffect("Heat");
    }
  }

  public void addGuage(float val) {
    if (superheatRunning || isTransforming) return;
    targetGuage -= val / maxGuage;
    // targetGuage += val / maxGuage;
  }

  // public void addBonus() {
  //   targetBonusGuage += 1.0f / maxNumTouch;
  //   Camera.main.GetComponent<CameraMover>().shake(0.1f, 7);
  // }

  public void startSuperheat() {
    if (isTransforming) return;

    superHeatCount++;
    // guage.fillAmount = 1;

    // ptb.show(false);

    // guageIcon.transform.localScale = guageScaleUp * Vector3.one;
    // guage.transform.localScale = guageScaleUp * Vector3.one;
    // guageShell.transform.localScale = guageScaleUp * Vector3.one;
    // guageColor.a = guageAlphaTurnedOn;
    // guageIcon.color = guageColor;
    // guage.color = guageColor;
    // guageIcon.transform.Find("Particle").gameObject.SetActive(true);

    isTransforming = true;
    transformStatus = 1;
    superXPos = superImage.anchoredPosition.x;
    heatXPos = heatImage.anchoredPosition.x;

    powerBoostBackground.startPowerBoost();
    superheatMover.gameObject.SetActive(true);
  }

  public void setStatus() {
    isTransforming = false;
    superheatRunning = true;
  }

  void stopSuperheat() {
    superheatMover.gameObject.SetActive(false);

    // ptb.show(true);
    superheatRunning = false;
    activated = false;
    targetGuage = 1;
    // targetBonusGuage = 0;

    powerBoostBackground.stopPowerBoost();

    // guageIcon.transform.localScale = Vector3.one;
    // guage.transform.localScale = Vector3.one;
    // guageShell.transform.localScale = Vector3.one;
    // guageIcon.transform.Find("Particle").gameObject.SetActive(false);
    // guageColor.a = guageAlphaDown;
    // guage.color = guageColor;
    // guageIcon.color = guageColor;
    // guageAlphaGoingUp = true;

    superImage.anchoredPosition = new Vector2(startSuperPos, 0);
    heatImage.anchoredPosition = new Vector2(startHeatPos, 0);
  }

  public GameObject getAfterImage() {
    for (int i = 0; i < afterImagePool.Count; i++) {
      if (!afterImagePool[i].activeInHierarchy) {
        return afterImagePool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(afterImagePrefab);
    afterImagePool.Add(obj);
    return obj;
  }

  public bool isOnSuperheat() {
    return superheatRunning || isTransforming;
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestNumSuperheats", superHeatCount);
  }

  // public bool canGetBonus() {
  //   return transformStatus == 2;
  // }

  public void fasterSuperheat(int val) {
    maxGuage *= (100 - val) / 100f;
  }

  public void longerSuperheat(int val) {
    boostDuration *= (100 + val) / 100f;
  }

  public void resetSuperheatAbility() {
    maxGuage = originalMaxGuage;
    boostDuration = originalBoostDuration;
  }
}
