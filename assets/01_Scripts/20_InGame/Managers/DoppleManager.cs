﻿using UnityEngine;
using System.Collections;

public class DoppleManager : ObjectsManager {
  public ParticleSystem getEnergy;
  public GameObject energyCube;
  public GameObject forceFieldPrefab;
  public GameObject forceFieldByDopplePrefab;
  public float[] forceFieldSizePerLevel;
  public float targetSize;
  public int loseEnergyAmount = 20;
  public AudioClip teleportSound;
  public float teleportSoundVolume = 0.5f;
  public AudioClip cannotTeleportWarningSound;
  public float waveAwakeDuration = 0.3f;

  public float blinkInterval = 3;
  public int blinkRadius = 50;
  public float teleportingDuration = 0.05f;
  public Color originalColor;

  override public void initRest() {
    int level = DataManager.dm.getInt("DoppleLevel") - 1;

    level = 2;

    targetSize = forceFieldSizePerLevel[level];

    skipInterval = true;
  }

  override protected void afterSpawn() {
    instance.GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
  }

  public float getTargetSize(bool byPlayer) {
    if (byPlayer) return targetSize;
    else return forceFieldSizePerLevel[0];
  }

}
