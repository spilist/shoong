using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SynchronizerData;

public class RhythmManager : MonoBehaviour {
  public static RhythmManager rm;
  // public AutoBoosterButton abb;
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
  public bool difficult;
  public float canBeMissedPosX = 0.3f;
  public float minBoosterOkPosX = 0.9f;
  public float minBoosterOkPosX_hard = -160;
  public float rightBeatPosX;
  public float maxBoosterOkPosX_hard = 160;
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
  private int remMax;
  private bool feverTime = false;
  public float autoFeverBoostPer = 0.2f;
  private bool skillActivated = false;

  private BeatObserver beatObserver;
  private bool beating = false;
  private bool gameStarted = false;
  private bool doingTutorial = false;

  private Transform origRhythmRings;
  private Text origTouchText;
  private Text origOnthebeatText;
  private Image origBoostImage;
  private GameObject fingerImage;
  public int failedBeatCount;
  public RhythmStar currentStar;
  public GameObject justSpawned;
  public GameObject feverPanel;
  private bool canBeMissed;

  public GameObject useSkillParticle;

  public delegate void RhythmCallback();
  private Dictionary<int, RhythmCallback> rhythmCallbackList;

	void Awake() {
    rm = this;
    beatObserver = GetComponent<BeatObserver>();
    normalColor = normalRing.GetComponent<Image>().color;
    rhythmCallbackList = new Dictionary<int, RhythmCallback>();
    origTouchText = touchText;
    origOnthebeatText = onthebeatText;
    origBoostImage = boostImage;
    origRhythmRings = rhythmRings;
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

  public void startBeat(Text touchText, Text ontheBeatText, Image boostImage, GameObject fingerImage, Transform rhythmRings) {
    beating = true;

    doingTutorial = true;
    this.touchText = touchText;
    this.onthebeatText = ontheBeatText;
    this.boostImage = boostImage;
    this.fingerImage = fingerImage;
    this.rhythmRings = rhythmRings;
    tutorialUI = rhythmRings.parent;
  }

  public void startGame() {
    beating = true;
    gameStarted = true;
    doingTutorial = false;
    touchText = origTouchText;
    onthebeatText = origOnthebeatText;
    boostImage = origBoostImage;
    rhythmRings = origRhythmRings;
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
      list.Add(ring);
    }

    justSpawned = ring;
    ring.transform.SetParent(rhythmRings, false);
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

	void invokeRing()
  {
    runRhythmCallbacks();
    if (!gameStarted || numSkillInLoop == 0) {
      Player.pl.shootBooster();

      // getRing(normalRingPool, normalRing);
    } else if (!feverTime) {
      rem = ringCount % (numNormalInLoop + numSkillInLoop);
      remMax = numNormalInLoop + numSkillInLoop - 1;
      ringCount++;

      // if (Player.pl.uncontrollable()) return;
      Debug.Log("Normal: " + numNormalInLoop + ", Skill: " + numSkillInLoop + ", rem: " + rem);
      // skill 나오기 전 파티클
      showPreSkillParticle(rem + 1);

      if (rem == 1 && skillActivated && numSkillInLoop > 0) {
        if (!SkillManager.sm.current().hasDuration()) {
          skillActivated = false;
          SkillManager.sm.stopSkills();
        }
      }

      if (rem < numNormalInLoop) {
        isSkillOK = false;
      } else {
        isSkillOK = true;
      }

      Player.pl.shootBooster();
    }
  }

  void showPreSkillParticle(int val) {
    if (numSkillInLoop == 0) return;

    if ( (numSkillInLoop == 1 && val == numNormalInLoop) || (numSkillInLoop > 1 && val >= numNormalInLoop && val < remMax + 1 ) ){
      float beatPeriod = beatObserver.beatPeriod;
      ParticleSystem particle = useSkillParticle.GetComponent<ParticleSystem>();
      particle.startLifetime = beatPeriod - particle.duration;
      particle.startSpeed = -21.6f / particle.startLifetime;
      float pitchCoeff = 0.437f;
      useSkillParticle.GetComponent<AudioSource>().pitch = beatPeriod * pitchCoeff * 2f;
      useSkillParticle.gameObject.SetActive(false);
      useSkillParticle.gameObject.SetActive(true);
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
    // feverPanel.SetActive(val);

    if (val) {
      isSkillOK = false;
      turnBoostOK(false);

      StartCoroutine("autoFeverBooster");
      // if (abb != null && abb.isOn()) StartCoroutine("autoFeverBooster");
    } else {
      StopCoroutine("autoFeverBooster");
    }

    // boostImage.gameObject.SetActive(!val);
  }

  IEnumerator autoFeverBooster() {
    while(true) {
      Player.pl.shootBooster();
      yield return new WaitForSeconds(autoFeverBoostPer);
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

  public void setCanBeMissed(bool val) {
    canBeMissed = val;
  }

  public void setDifficulty(bool difficult) {
    this.difficult = difficult;
  }

  public float minOK() {
    if (difficult) return minBoosterOkPosX_hard;
    else return minBoosterOkPosX;
  }

  public float maxOK() {
    if (difficult) return maxBoosterOkPosX_hard;
    else return maxBoosterOkPosX;
  }

  public void registerCallback(int instanceId, RhythmCallback callback)
  {
    if (rhythmCallbackList.ContainsKey(instanceId))
    {
      Debug.LogError("Dulicate intanceId is detected!!");
    }
    rhythmCallbackList.Add(instanceId, callback);
  }

  public void unregisterCallback(int instanceId)
  {
    rhythmCallbackList.Remove(instanceId);
  }

  private void runRhythmCallbacks()
  {
    foreach(RhythmCallback callback in rhythmCallbackList.Values)
    {
      callback();
    }
  }
}
