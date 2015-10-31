using UnityEngine;
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
  public int ringAmount = 2;
  public List<GameObject> normalRingPool;
  public List<GameObject> skillRingPool;
  public List<GameObject> failedRhythmPool;

  public float maxBoosterOkScale = 4;
  public float maxPopScale;
  public float rightBeatScale = 3;
  public float minPopScale;
  public float minBoosterOkScale = 2;
  public float ringDisppearDuration = 0.5f;
  public float playerScaleUpAmount = 1.2f;

  private int numNormalInLoop;
  private int numSkillInLoop;

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
  private bool isAfterRing = false;
  private bool ringSuccess = false;

  private BeatObserver beatObserver;
  private bool beating = false;
  private bool gameStarted = false;

	void Awake() {
    rm = this;
    beatObserver = GetComponent<BeatObserver>();
    normalColor = normalRing.GetComponent<SpriteRenderer>().color;
  }

  void Start() {
    normalRingPool = new List<GameObject>();
    skillRingPool = new List<GameObject>();
    failedRhythmPool = new List<GameObject>();

    for (int i = 0; i < ringAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(normalRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      normalRingPool.Add(obj);

      obj = (GameObject) Instantiate(skillRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      skillRingPool.Add(obj);

      obj = (GameObject) Instantiate(failedRhythmPrefab);
      obj.SetActive(false);
      obj.transform.SetParent(inGameUI, false);
      failedRhythmPool.Add(obj);
    }

    beating = true;
  }

  public void startGame() {
    gameStarted = true;
  }

  public void setPeriod(float val) {
    samplePeriod = val;
  }

  void Update () {
    if (beating && !Player.pl.uncontrollable() && (beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
      invokeRing();
    }
  }

  public void stopBeat() {
    beating = false;
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
      ring.transform.parent = rhythmRings;
      list.Add(ring);
    }

    ring.transform.localPosition = Vector3.zero;
    lastRing = currentRing;
    currentRing = ring;
    ring.SetActive(true);
  }

  void rhythmFailed(string text) {
    GameObject failed = null;
    for (int i = 0; i < failedRhythmPool.Count; i++) {
      if (!failedRhythmPool[i].activeInHierarchy) {
        failed = failedRhythmPool[i];
        break;
      }
    }

    if (failed == null) {
      failed = (GameObject) Instantiate(failedRhythmPrefab);
      failed.transform.SetParent(inGameUI, false);
      failedRhythmPool.Add(failed);
    }

    failed.GetComponent<Text>().text = text;
    failed.SetActive(true);
  }

	void invokeRing() {
    if (!gameStarted || numSkillInLoop == 0) {
      getRing(normalRingPool, normalRing);
    } else if (!feverTime) {
      rem = ringCount % (numNormalInLoop + numSkillInLoop);

      if (rem == 1 && skillActivated) {
        Debug.Log("stopped");
        skillActivated = false;
        SkillManager.sm.stopSkills();
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
    }
  }

  public void afterRing(bool val) {
    isAfterRing = val;
  }

  public bool canBoost() {
    return feverTime || isBoosterOK;
  }

  void resetSkillRings() {
    if (skillActivated) {
      ringCount = 0;
      SkillManager.sm.stopSkills();
      skillActivated = false;

      if (isAfterRing) {
        lastRing.GetComponent<RhythmRing>().disappear();
        currentRing.GetComponent<RhythmRing>().skillRing = false;
        currentRing.GetComponent<SpriteRenderer>().color = normalColor;
      }
    }
  }

  public void ringSuccessed() {
    ringSuccess = true;
  }

  public void ringMissed() {
    ringSuccess = false;
    rhythmFailed("Missed");
    if (skillActivated) Debug.Log("Failed");
    resetSkillRings();

    if (!isAfterRing) {
      currentRing.GetComponent<RhythmRing>().disappear();
    }
  }

  public void ringSkipped(bool skillRing) {
    if (!gameStarted) return;

    if (ringSuccess) {
      ringSuccess = false;
      return;
    }

    rhythmFailed("Skipped");

    if (skillRing) {
      if (skillActivated) Debug.Log("skipped");
      resetSkillRings();
    }
  }

  public void setLoop(int normal, int skill) {
    numNormalInLoop = normal;
    numSkillInLoop = skill;
  }

  public void setFever(bool val) {
    feverTime = val;
    Player.pl.scaleChange(val, playerScaleUpAmount);
    if (val) {
      isSkillOK = false;
    }
  }

  public void loopSkillActivated(bool val) {
    skillActivated = val;
  }
}
