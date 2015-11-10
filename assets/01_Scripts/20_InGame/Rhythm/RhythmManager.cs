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
  public Transform tutorialUI;
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
  private bool isAfterRing = false;
  private bool ringSuccess = false;

  private BeatObserver beatObserver;
  private bool beating = false;
  private bool gameStarted = false;
  private bool doingTutorial = false;

  private Text origTouchText;
  private Text origOnthebeatText;
  private Image origBoostImage;
  private GameObject fingerImage;

	void Awake() {
    rm = this;
    beatObserver = GetComponent<BeatObserver>();
    normalColor = normalRing.GetComponent<SpriteRenderer>().color;

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
      obj.transform.parent = rhythmRings;
      normalRingPool.Add(obj);

      obj = (GameObject) Instantiate(skillRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      skillRingPool.Add(obj);

      obj = (GameObject) Instantiate(failedRhythmPrefab);
      obj.SetActive(false);
      failedRhythmPool.Add(obj);
    }

    if (DataManager.dm.getBool("TutorialDone")) beating = true;
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
    if (beating && !Player.pl.uncontrollable() && (beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
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

      if (rem == 1 && skillActivated) {
        if (SkillManager.sm.current().name != "Shield") {
          // Debug.Log("stopped");
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
    }
  }

  public void afterRing(bool val) {
    isAfterRing = val;
  }

  public bool canBoost() {
    return feverTime || isBoosterOK;
  }

  void resetSkillRings() {
    if (skillActivated && numSkillInLoop > 1) {
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
    rhythmFailed("MISSED");
    // if (skillActivated) Debug.Log("Failed");
    resetSkillRings();

    if (!isAfterRing) {
      currentRing.GetComponent<RhythmRing>().disappear();
    }
  }

  public void ringSkipped(bool skillRing) {
    if (!doingTutorial && !gameStarted) return;

    if (ringSuccess) {
      ringSuccess = false;
      return;
    }

    rhythmFailed("SKIPPED");

    if (skillRing) {
      // if (skillActivated) Debug.Log("skipped");
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
    if (val) {
      isSkillOK = false;
    }
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
}
