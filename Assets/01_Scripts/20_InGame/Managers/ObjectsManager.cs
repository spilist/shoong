﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsManager : MonoBehaviour {
  public bool forceSpawnAtStart = false;
  public int objAmount = 1;
  public List<GameObject> objPool;
  public List<GameObject> objDestroyEffectPool;
  public List<GameObject> objEncounterEffectPool;
  public GameObject objPrefab;
  public GameObject objDestroyEffect;
  public GameObject objEncounterEffect;
  public ParticleSystem objEncounterEffectForPlayer;
  public GameObject strengthenPlayerEffect;

  public float strength = 0;
  public float speed = 0;
  public float tumble = 1;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;
  public int cubesByEncounter;
  public float energyGets;
  public int numSweetsPop;
  public bool isNegative = false;
  public bool destroyWhenCollideSelf;

  protected SpawnManager spawnManager;
  protected bool skipInterval = false;

  public GameObject instance;
  public int loseEnergyWhenEncounter;
  public float bounceDuration;
  public bool hasLevel = false;
  public string nameForLevel;
  public int level;
  public Player player;

  protected bool spawnedByTransform = false;

  // This action method would applied to return value of getPooledObj()
  protected System.Action<GameObject> applyWhenGettingObj = null;

  virtual protected void beforeInit() {}

  void Awake() {
    player = Player.pl;
  }

  void OnEnable() {
    beforeInit();
    spawnManager = gameObject.GetComponent<SpawnManager>();

    if (forceSpawnAtStart) {
      minSpawnInterval = 0;
      maxSpawnInterval = 0;
    }

    if (hasLevel) {
      adjustForLevel(3);
    }

    objPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(objPrefab);
      obj.SetActive(false);
      obj.transform.parent = transform;
      objPool.Add(obj);
    }

    if (objDestroyEffect != null) {
      objDestroyEffectPool = new List<GameObject>();
      for (int i = 0; i < objAmount; ++i) {
        GameObject obj = (GameObject) Instantiate(objDestroyEffect);
        obj.SetActive(false);
        objDestroyEffectPool.Add(obj);
      }
    }

    if (objEncounterEffect != null) {
      objEncounterEffectPool = new List<GameObject>();
      for (int i = 0; i < objAmount; ++i) {
        GameObject obj = (GameObject) Instantiate(objEncounterEffect);
        obj.SetActive(false);
        objEncounterEffectPool.Add(obj);
      }
    }

    initRest();
  }

  public void spawnPooledObjs(List<GameObject> list, GameObject prefab, int count) {
    for (int i = 0; i < count; i++) {
      GameObject obj = getPooledObj(list, prefab, spawnManager.getSpawnPosition(prefab));
      obj.SetActive(true);
    }
  }

  public GameObject getPooledObj(List<GameObject> list, GameObject prefab, Vector3 pos) {
    GameObject obj = getPooledObj(list, prefab);
    obj.transform.parent = transform;
    obj.transform.position = pos;
    if (applyWhenGettingObj != null)
      applyWhenGettingObj(obj);
    return obj;
  }

  public GameObject getPooledObj(List<GameObject> list, GameObject prefab) {
    for (int i = 0; i < list.Count; i++) {
      if (!list[i].activeInHierarchy) {
        return list[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(prefab);
    obj.transform.parent = transform;
    list.Add(obj);
    if (applyWhenGettingObj != null)
      applyWhenGettingObj(obj);
    return obj;
  }

  virtual public void initRest() {}

  virtual public void run() {
    if (strengthenPlayerEffect != null) {
      strengthenPlayerEffect.SetActive(false);
    }
    StartCoroutine(respawnRoutine());
  }

  public void runByTransform(Vector3 pos) {
    if (instance != null && instance.activeSelf) {
      instance.GetComponent<ObjectsMover>().destroyObject(false, false, false);
    }
    beforeSpawn();
    instance = getPooledObj(objPool, objPrefab, pos);
    instance.SetActive(true);
    spawnedByTransform = true;
    afterSpawn();
  }

  virtual public void runImmediately() {
    skipInterval = true;
    StartCoroutine(respawnRoutine());
  }

  virtual public void adjustForLevel(int level) {}

  protected IEnumerator respawnRoutine() {
    if (ScoreManager.sm.isGameOver()) yield break;

    yield return new WaitForSeconds(spawnInterval());

    skipInterval = false;

    if (spawnedByTransform) {
      spawnedByTransform = false;
      yield break;
    }

    if (checkAlreadyRunning()) yield break;
    beforeSpawn();
    spawn();
    afterSpawn();
  }

  virtual public bool checkAlreadyRunning() {
    return false;
  }

  virtual public void respawn() {
    if (ScoreManager.sm.isGameOver()) return;

    int count = objAmount - GameObject.FindGameObjectsWithTag(objPrefab.tag).Length;

    if (count > 0) {
      spawnPooledObjs(objPool, objPrefab, count);
    }
  }

  virtual protected float spawnInterval() {
    if (skipInterval) return 0;
    else return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  virtual protected void spawn() {
    if (ScoreManager.sm.isGameOver()) return;

    instance = getPooledObj(objPool, objPrefab, spawnManager.getSpawnPosition(objPrefab));
    instance.SetActive(true);
  }

  virtual protected void afterSpawn() {}
  virtual protected void beforeSpawn() {}

  virtual public float getSpeed() {
    return speed;
  }

  virtual public float getTumble() {
    return tumble;
  }

  virtual public Vector3 getDirection() {
    Vector2 randomV = Random.insideUnitCircle;
    return new Vector3(randomV.x, 0, randomV.y).normalized;
  }

  public void skipRespawnInterval() {
    skipInterval = true;
  }

  virtual public int cubesWhenEncounter() {
    return cubesByEncounter;
  }

  public void stopRespawn() {
    StopCoroutine("respawnRoutine");
  }

}
