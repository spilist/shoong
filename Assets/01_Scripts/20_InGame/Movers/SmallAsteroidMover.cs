﻿using UnityEngine;
using System.Collections;

public class SmallAsteroidMover : ObjectsMover {
  SmallAsteroidManager sam;
  private int minBrokenSpawn;
  private int maxBrokenSpawn;
  private float minBrokenSize;
  private float maxBrokenSize;

  override public string getManager() {
    return "SmallAsteroidManager";
  }

  override protected void initializeRest() {
    sam = (SmallAsteroidManager)objectsManager;
    canBeMagnetized = false;
    minBrokenSpawn = sam.minBrokenSpawn;
    maxBrokenSpawn = sam.maxBrokenSpawn;
    minBrokenSize = sam.minBrokenSize;
    maxBrokenSize = sam.maxBrokenSize;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override public void showDestroyEffect(bool byPlayer) {
    for (int howMany = Random.Range(minBrokenSpawn, maxBrokenSpawn + 1); howMany > 0; howMany--) {
      GameObject broken = sam.getPooledObj(objectsManager.objDestroyEffectPool, objectsManager.objDestroyEffect, transform.position);
      broken.SetActive(true);
      broken.transform.localScale = Random.Range(minBrokenSize, maxBrokenSize) * Vector3.one;
      broken.GetComponent<MeshFilter>().sharedMesh = sam.getRandomMesh();

      if (howMany == 1) broken.GetComponent<AudioSource>().Play();
    }
  }

  override public void showEncounterEffect() {
    showDestroyEffect(true);
  }
}