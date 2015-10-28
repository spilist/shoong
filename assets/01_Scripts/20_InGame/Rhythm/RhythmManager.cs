using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RhythmManager : MonoBehaviour {
  public static RhythmManager rm;
  public Transform rhythmCircles;
  public GameObject rhythmCircle;
  public int circleAmount = 5;
  public List<GameObject> circlePool;

  public float beatScaleDiff = 0.4f;
  public float bpm;
  public float beatBase;
  public float invokeCirclePer;
  public float boosterOkScale = 1;
  public bool isBoosterOK = false;
  public GameObject currentCircle;

	void Awake() {
    rm = this;
  }

  void Start() {
    circlePool = new List<GameObject>();
    for (int i = 0; i < circleAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(rhythmCircle);
      obj.SetActive(false);
      obj.transform.parent = rhythmCircles;
      circlePool.Add(obj);
    }

    calculateBeat(beatBase);
    InvokeRepeating("invokeCircle", 0, invokeCirclePer);
  }

  void calculateBeat(float val) {
    invokeCirclePer = val * 60.0f / bpm;
  }

  public void stopBeat() {
    CancelInvoke();
  }

  public void newBeat(float val) {
    calculateBeat(val);
    CancelInvoke();
    InvokeRepeating("invokeCircle", 0, invokeCirclePer);
  }

	void invokeCircle() {
    Player.pl.startBeat();

    GameObject circle = null;
    for (int i = 0; i < circlePool.Count; i++) {
      if (!circlePool[i].activeInHierarchy) {
        circle = circlePool[i];
        break;
      }
    }

    if (circle == null) {
      circle = (GameObject) Instantiate(rhythmCircle);
      circle.transform.parent = rhythmCircles;
      circlePool.Add(circle);
    }

    circle.transform.localPosition = Vector3.zero;
    currentCircle = circle;
    circle.SetActive(true);
  }

  void OnDisable() {
    CancelInvoke();
  }

  public void boosterOk(bool val) {
    isBoosterOK = val;
  }
}
