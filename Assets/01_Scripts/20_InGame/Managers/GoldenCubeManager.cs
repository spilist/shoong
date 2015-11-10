﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoldenCubeManager : ObjectsManager {
  public float spawnRadius = 200;
  public float detectDistance = 200;

  override public void initRest() {
    run();
  }

  override protected void spawn() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);

    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.SetActive(true);
  }

  override protected float spawnInterval() {
    return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  public void goldenDestroyEffect(Vector3 pos) {
    GameObject obj = getPooledObj(objDestroyEffectPool, objDestroyEffect, pos);
    obj.SetActive(true);
  }

  public void spawnGoldenCube(Vector3 pos) {
    GameObject obj = getPooledObj(objPool, objPrefab, pos);
    obj.SetActive(true);
    obj.GetComponent<GoldenCubeMover>().setNoRespawn();
  }
}