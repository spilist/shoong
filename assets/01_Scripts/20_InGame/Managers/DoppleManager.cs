using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoppleManager : ObjectsManager {
  public Transform blinkForces;
  public int blinkDistance = 100;
  public ParticleSystem getEnergy;
  public GameObject energyCube;
  public GameObject forceFieldPrefab;
  public List<GameObject> goodFieldPool;
  public GameObject forceFieldByDopplePrefab;
  public List<GameObject> badFieldPool;
  public int fieldAmount = 30;

  public float[] forceFieldSizePerLevel;
  public float targetSize;
  public AudioClip teleportSound;
  public float teleportSoundVolume = 0.5f;
  public AudioClip cannotTeleportWarningSound;
  public float waveAwakeDuration = 0.3f;

  public float blinkInterval = 3;
  public int blinkRadius = 50;

  override public void initRest() {
    goodFieldPool = new List<GameObject>();
    badFieldPool = new List<GameObject>();

    for (int i = 0; i < fieldAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(forceFieldPrefab);
      obj.SetActive(false);
      goodFieldPool.Add(obj);

      obj = (GameObject) Instantiate(forceFieldByDopplePrefab);
      obj.SetActive(false);
      badFieldPool.Add(obj);
    }
  }

  override public void adjustForLevel(int level) {
    targetSize = forceFieldSizePerLevel[level];
  }

  public float getTargetSize(bool byPlayer) {
    if (byPlayer) return targetSize;
    else return forceFieldSizePerLevel[0];
  }

  public void goodFieldAt(Vector3 pos) {
    GameObject obj = getPooledObj(goodFieldPool, forceFieldPrefab, pos);
    obj.SetActive(true);
  }

  public void goodFieldAt() {
    GameObject obj = getPooledObj(goodFieldPool, forceFieldPrefab);
    obj.transform.SetParent(blinkForces, false);
    obj.SetActive(true);
  }

}
