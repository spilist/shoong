using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SynchronizerData;

public class RhythmManager : MonoBehaviour {
  public static RhythmManager rm;
  public Transform rhythmRings;
  public GameObject normalRing;
  public GameObject skillRing;
  public GameObject feverRing;
  public int ringAmount = 2;
  public List<GameObject> normalRingPool;
  public List<GameObject> skillRingPool;
  public List<GameObject> feverRingPool;

  public float bpm;
  public float beatBase;
  public float boosterOkScale = 1;

  private int numNormalInLoop;
  private int numSkillInLoop;

  public float samplePeriod;
  public bool isBoosterOK = false;
  public bool isSkillOK = false;
  public GameObject currentRing;
  private int ringCount = 0;
  private bool feverTime = false;

  private BeatObserver beatObserver;
  private bool beating = false;

	void Awake() {
    rm = this;
    beatObserver = GetComponent<BeatObserver>();
  }

  void Start() {
    normalRingPool = new List<GameObject>();
    skillRingPool = new List<GameObject>();
    feverRingPool = new List<GameObject>();
    for (int i = 0; i < ringAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(normalRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      normalRingPool.Add(obj);

      obj = (GameObject) Instantiate(skillRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      skillRingPool.Add(obj);

      obj = (GameObject) Instantiate(feverRing);
      obj.SetActive(false);
      obj.transform.parent = rhythmRings;
      feverRingPool.Add(obj);
    }

    beating = true;
  }

  public void setPeriod(float val) {
    samplePeriod = val;
  }

  void Update () {
    if (beating && (beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat) {
      invokeRing();
    }
  }

  public void stopBeat() {
    beating = false;
  }

  GameObject getRing(List<GameObject> list, GameObject prefab) {
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
    currentRing = ring;
    ring.SetActive(true);
    return ring;
  }

	void invokeRing() {
    if (numSkillInLoop > 0) {
      if (!feverTime) {
        int rem = ringCount % (numNormalInLoop + numSkillInLoop);
        if (rem < numNormalInLoop) {
          getRing(normalRingPool, normalRing);
        } else {
          getRing(skillRingPool, skillRing);
        }
        ringCount++;
      }
    } else {
      getRing(normalRingPool, normalRing);
    }
  }

  void OnDisable() {
    stopBeat();
  }

  public void boosterOk(bool boosterRing, bool skillRing) {
    isBoosterOK = boosterRing;
    isSkillOK = skillRing;
  }

  public bool canBoost() {
    return feverTime || isBoosterOK;
  }

  public void ringMissed() {
    currentRing.SetActive(false);
  }

  public void setLoop(int normal, int skill) {
    numNormalInLoop = normal;
    numSkillInLoop = skill;
  }

  public void setFever(bool val) {
    feverTime = val;
    if (val) spawnFeverRing();
  }

  public void spawnFeverRing() {
    getRing(feverRingPool, feverRing);
  }
}
