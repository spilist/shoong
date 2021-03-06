﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : ObjectsManager {
  public float speed_runaway = 120;
  public float speed_weaken = 30;

  public float spawnRadius = 600;
  public float detectDistance = 160;

  public float minLifeTime = 10;
  public float maxLifeTime = 15;

  public float weakenDuration = 5.5f;

  public float shrinkUntil = 1.2f;
  public float shrinkSpeed = 2;

  public GameObject minimonPrefab;
  public List<GameObject> minimonPool;
  public List<GameObject> minimonDestroyPool;

  public int numMinimonRespawn = 4;
  public int minimonAdditionalSpeed = 20;
  public float minimonStartTimeByMonster = 0.5f;
  public float minimonStartTimeByPlayer = 1;
  public float minimonStartSpeedByMonster = 40;
  public float minimonStartSpeedByPlayer = 80;
  public float minimonTumble = 10;
  public float minimonLifeTime = 4;
  public int minimonLoseEnergy = 10;
  public float minimonBounceDuration = 0.05f;

  public int cubesWhenDestroyMinimon = 5;
  public int[] numsMinimonSpawn;
  public int numMinimonSpawn;

  public int maxEnlargeCount = 50;
  public float enlargeScalePerMinimon = 0.01f;
  public int enlargeSpeedPerMinimon = 2;

  public GameObject minimonDestroyEffect;

  public Color weakenedOutlineColor;
  public GameObject monsterFilter;
  public OffscreenObjectIndicator indicator;

  public GameObject monsterWarning;
  public float warningBlinkSeconds = 0.7f;

	override public void initRest() {
    minimonPool = new List<GameObject>();
    minimonDestroyPool = new List<GameObject>();
    for (int i = 0; i < numMinimonRespawn; ++i) {
      GameObject obj = (GameObject) Instantiate(minimonPrefab);
      obj.SetActive(false);
      obj.transform.parent = transform;
      minimonPool.Add(obj);

      obj = (GameObject) Instantiate(minimonDestroyEffect);
      obj.SetActive(false);
      minimonDestroyPool.Add(obj);
    }
  }

  public void spawnMinimon(Vector3 pos) {
    GameObject obj = getPooledObj(minimonPool, minimonPrefab, pos);
    obj.SetActive(true);
  }

  public void spawnMinimon(Vector3 pos, int count) {
    for (int i = 0; i < count; i++) {
      spawnMinimon(pos);
    }
  }

  public void destroyMinimon(Vector3 pos) {
    GameObject obj = getPooledObj(minimonDestroyPool, minimonDestroyEffect, pos);
    obj.SetActive(true);
  }

  override public void adjustForLevel(int level) {
    numMinimonSpawn = numsMinimonSpawn[level - 1];
  }

  override public void runImmediately() {
    skipInterval = false;
    StartCoroutine(respawnRoutine());
  }

  override protected void spawn() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  override protected void afterSpawn() {
    StartCoroutine("startWarning");
  }

  IEnumerator startWarning() {
    monsterWarning.GetComponent<AudioSource>().Play();
    while(true) {
      monsterWarning.GetComponent<Text>().enabled = true;

      yield return new WaitForSeconds(warningBlinkSeconds);

      monsterWarning.GetComponent<Text>().enabled = false;

      yield return new WaitForSeconds(1 - warningBlinkSeconds);
    }
  }

  public void stopWarning() {
    monsterWarning.GetComponent<AudioSource>().Stop();
    monsterWarning.GetComponent<Text>().enabled = false;
    StopCoroutine("startWarning");
  }

  override public Vector3 getDirection() {
    Vector3 dir = player.transform.position - instance.transform.position;
    return dir / dir.magnitude;
  }

  public void stopRiding() {
    monsterFilter.SetActive(false);
    objEncounterEffectForPlayer.Play();
    objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();
    foreach (GameObject mm in GameObject.FindGameObjectsWithTag("MiniMonster")) {
      mm.GetComponent<MiniMonsterMover>().destroyObject();
    }
  }
}
