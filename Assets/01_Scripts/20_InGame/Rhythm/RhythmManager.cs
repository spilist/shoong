﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SynchronizerData;

public class RhythmManager : MonoBehaviour {
  public static RhythmManager rm;
  public Transform rhythmRings;
  public GameObject normalRing;
  public GameObject skillRing;
  public GameObject failedRhythmPrefab;
  public Transform inGameUI;
  public Transform tutorialUI;
  public int ringAmount = 2;
  private List<GameObject> normalRingPool;
  private List<GameObject> skillRingPool;
  private List<GameObject> failedRhythmPool;

  // public float scaleBase;
  // public float maxBoosterOkScale = 4;
  // public float maxPopScale;
  // public float rightBeatScale = 3;
  // public float minPopScale;
  // public float minBoosterOkScale = 2;
  public float ringDisppearDuration = 0.5f;
  // public float playerScaleUpAmount = 1.2f;
  public float canBeMissedPosX = 0.3f;
  public float minBoosterOkPosX = 0.9f;
  public float rightBeatPosX;
  public float maxBoosterOkPosX = 1.1f;

  private int numNormalInLoop;
  private int numSkillInLoop;

  public Text touchText;
  public Text onthebeatText;
  public Image boostImage;
  public Color inactiveBoostImageColor;
  public Color activeBoostImageColor;

  public Color normalColor;
  public float samplePeriod;
  public bool isBoosterOK = false;
  public bool isSkillOK = false;
  public GameObject currentRing;
  private GameObject lastRing;
  private int ringCount = 0;
  private int rem;
  private bool feverTime = false;
  private bool skillActivated = false;

  private BeatObserver beatObserver;
  private bool beating = false;
  private bool gameStarted = false;
  private bool doingTutorial = false;

  private Text origTouchText;
  private Text origOnthebeatText;
  private Image origBoostImage;
  private GameObject fingerImage;
  public int failedBeatCount;
  public RhythmStar currentStar;
  public GameObject justSpawned;
  public GameObject feverPanel;
  private bool canBeMissed;

	void Awake() {
    rm = this;
    beatObserver = GetComponent<BeatObserver>();
    normalColor = normalRing.GetComponent<Image>().color;

    origTouchText = touchText;
    origOnthebeatText = onthebeatText;
    origBoostImage = boostImage;
  }

  void Start() {
    normalRingPool = new List<GameObject>();
    skillRingPool = new List<GameObject>();
    failedRhythmPool = new List<GameObject>();

    for (int i = 0; i < ringAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(normalRing);
      obj.SetActive(false);
      obj.transform.SetParent(rhythmRings, false);
      normalRingPool.Add(obj);

      obj = (GameObject) Instantiate(skillRing);
      obj.SetActive(false);
      obj.transform.SetParent(rhythmRings, false);
      skillRingPool.Add(obj);

      obj = (GameObject) Instantiate(failedRhythmPrefab);
      obj.SetActive(false);
      failedRhythmPool.Add(obj);
    }
  }

  public void startBeat(Text touchText, Text ontheBeatText, Image boostImage, GameObject fingerImage) {
    beating = true;

    if (tutorialUI.gameObject.activeInHierarchy) doingTutorial = true;

    this.touchText = touchText;
    this.onthebeatText = ontheBeatText;
    this.boostImage = boostImage;
    this.fingerImage = fingerImage;
  }

  public void startGame() {
    beating = true;
    gameStarted = true;
    doingTutorial = false;
    touchText = origTouchText;
    onthebeatText = origOnthebeatText;
    boostImage = origBoostImage;
  }

  public void setPeriod(float val) {
    samplePeriod = val;
  }

  void Update () {
    if (beating && (beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
      invokeRing();
    }
  }

  public void stopBeat(bool val = false) {
    beating = val;
  }

  void getRing(List<GameObject> list, GameObject prefab) {
    GameObject ring = null;
    for (int i = 0; i < list.Count; i++) {
      if (!list[i].activeInHierarchy) {
        ring = list[i];
        break;
      }
    }

    if (ring == null) {
      ring = (GameObject) Instantiate(prefab);
      ring.transform.SetParent(rhythmRings, false);
      list.Add(ring);
    }

    justSpawned = ring;
    ring.SetActive(true);
  }

  void rhythmFailed(string text) {
    GameObject failed = null;
    failedBeatCount++;
    for (int i = 0; i < failedRhythmPool.Count; i++) {
      if (!failedRhythmPool[i].activeInHierarchy) {
        failed = failedRhythmPool[i];
        break;
      }
    }

    if (failed == null) {
      failed = (GameObject) Instantiate(failedRhythmPrefab);
      failedRhythmPool.Add(failed);
    }

    failed.GetComponent<Text>().text = text;

    if (doingTutorial) {
      failed.transform.SetParent(tutorialUI, false);
    } else {
      failed.transform.SetParent(inGameUI, false);
    }

    failed.SetActive(true);
  }

	void invokeRing() {
    if (!gameStarted || numSkillInLoop == 0) {
      getRing(normalRingPool, normalRing);
    } else if (!feverTime) {
      rem = ringCount % (numNormalInLoop + numSkillInLoop);

      if (rem == 1 && skillActivated && numSkillInLoop > 1) {
        if (SkillManager.sm.current().name != "Shield") {
          skillActivated = false;
          SkillManager.sm.stopSkills();
        }
      }

      if (rem < numNormalInLoop) {
        getRing(normalRingPool, normalRing);
      } else {
        getRing(skillRingPool, skillRing);
      }
      ringCount++;
    }
  }

  void OnDisable() {
    stopBeat();
  }

  public void boosterOk(bool boosterRing, bool skillRing) {
    if (!feverTime) {
      isBoosterOK = boosterRing;
      isSkillOK = skillRing;
      turnBoostOK(isBoosterOK);
    }
  }

  public bool canBoost() {
    return feverTime || isBoosterOK;
  }

  void resetSkillRings() {
    if (skillActivated && numSkillInLoop > 1) {
      ringCount = 0;
      SkillManager.sm.stopSkills();
      skillActivated = false;

      justSpawned.GetComponent<RhythmStar>().skillRing = false;
      justSpawned.GetComponent<Image>().color = normalColor;
    }
  }

  public void setCurrent(RhythmStar star) {
    currentStar = star;
    setCanBeMissed(true);
  }

  public void ringSuccessed() {
    isBoosterOK = false;
    isSkillOK = false;

    if (!feverTime) turnBoostOK(false);

    currentStar.success();
  }

  public void ringMissed() {
    if (!canBeMissed) return;

    canBeMissed = false;
    rhythmFailed("MISSED");
    resetSkillRings();

    currentStar.disappear();

    // justSpawned.GetComponent<RhythmStar>().disappear();
  }

  public void ringSkipped(bool skillRing) {
    if (!doingTutorial && !gameStarted) return;

    rhythmFailed("SKIPPED");

    if (skillRing) {
      resetSkillRings();
    }
  }

  public void setLoop(int normal, int skill) {
    numNormalInLoop = normal;
    numSkillInLoop = skill;
  }

  public void setFever(bool val) {
    feverTime = val;
    Player.pl.setFever(val);
    feverPanel.SetActive(val);

    if (val) {
      isSkillOK = false;
      turnBoostOK(false);
    }

    boostImage.gameObject.SetActive(!val);
  }

  public void loopSkillActivated(bool val) {
    skillActivated = val;
  }

  public void turnBoostOK(bool val) {
    if (val) {
      boostImage.color = activeBoostImageColor;
      touchText.color = activeBoostImageColor;
      onthebeatText.color = activeBoostImageColor;
    } else {
      boostImage.color = inactiveBoostImageColor;
      touchText.color = inactiveBoostImageColor;
      onthebeatText.color = inactiveBoostImageColor;
    }

    if (fingerImage != null) fingerImage.SetActive(val);
  }

  public void setCanBeMissed(bool val) {
    canBeMissed = val;
  }
}
