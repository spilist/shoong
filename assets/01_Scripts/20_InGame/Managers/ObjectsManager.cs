﻿using UnityEngine;
using System.Collections;

public class ObjectsManager : MonoBehaviour {
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
  public bool isNegative = false;

  protected SpawnManager spawnManager;
  public PlayerMover player;
  protected bool skipInterval = false;
  public ComboBar comboBar;

  public GameObject instance;
  public bool forceSpawnAtStart = false;

  void OnEnable() {
    spawnManager = gameObject.GetComponent<SpawnManager>();
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    comboBar = GameObject.Find("Bars Canvas").GetComponent<ComboBar>();

    initRest();

    if (forceSpawnAtStart) {
      minSpawnInterval = 0;
      maxSpawnInterval = 0;
    }
  }

  virtual public void initRest() {}

  virtual public void run() {
    if (strengthenPlayerEffect != null) {
      strengthenPlayerEffect.SetActive(false);
    }
    StartCoroutine(respawnRoutine());
  }

  virtual public void runImmediately() {
    skipInterval = true;
    StartCoroutine(respawnRoutine());
  }

  virtual public IEnumerator respawnRoutine() {
    yield return new WaitForSeconds(spawnInterval());

    skipInterval = false;

    spawn();
    afterSpawn();
  }

  virtual protected float spawnInterval() {
    if (skipInterval) return 0;
    else return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  virtual protected void spawn() {
    instance = spawnManager.spawn(objPrefab);
  }

  virtual protected void afterSpawn() {}

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
    if (isNegative) return cubesByEncounter;
    else return comboBar.getComboRatio();
  }

}
