﻿using UnityEngine;
using System.Collections;

public class CubeDispenserMover : ObjectsMover {
  private CubeDispenserManager cdm;
  private int comboCount = 0;
  private int brokenCount = 0;

  protected override void initializeRest() {
    canBeMagnetized = false;
    cdm = (CubeDispenserManager)objectsManager;
  }

  public void tryBreak() {
    if (brokenCount == 2) {
      destroyObject(true, true);
      player.goodPartsEncounter(this, cdm.cubesByEncounter * cdm.fullComboCount);
      player.setTrapped(false);
      QuestManager.qm.addCountToQuest("ExitCubeDispenser");
      return;
    }

    Camera.main.GetComponent<CameraMover>().shake();
    cdm.tryBreak();

    GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[brokenCount];
    brokenCount++;
  }

  override protected void afterDestroy(bool byPlayer) {
    player.setTrapped(false);

    if (byPlayer && comboCount != cdm.fullComboCount) DataManager.dm.increment("NumDestroyCubeDispenser");
  }

  override protected void afterCollide(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      if (player.isUsingRainbow() || player.isUnstoppable()) {
        player.goodPartsEncounter(this, cdm.cubesByEncounter * cdm.fullComboCount);
      } else {
        player.contactCubeDispenser(transform, cdm.cubesByEncounter, collision, cdm.reboundDuring);

        encounterPlayer(false);
      }
    }
  }

  override protected bool beforeEncounter() {
    cdm.checkTrying();

    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cdm.pitchStart + comboCount * cdm.pitchIncrease;

    GetComponent<ParticleSystem>().emissionRate -= cdm.decreaseEmissionAmount;

    comboCount++;
    Camera.main.GetComponent<CameraMover>().shake(cdm.shakeDurationByHit, cdm.shakeAmountByHit);

    if (comboCount == cdm.fullComboCountPerLevel[0]) {
      QuestManager.qm.addCountToQuest("CubeDispenser");
    }

    if (comboCount == cdm.fullComboCount - 4) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[0];

    if (comboCount == cdm.fullComboCount - 2) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[1];

    if (comboCount == cdm.fullComboCount) {
      QuestManager.qm.addCountToQuest("CompleteCubeDispenser");
      destroyObject(true, true);
      player.showEffect("Great");
      DataManager.dm.increment("NumCompleteCubeDispenser");
      return false;
    }

    return true;
  }

  override public string getManager() {
    return "CubeDispenserManager";
  }

  override public int cubesWhenEncounter() {
    return cdm.cubesByEncounter * cdm.fullComboCount;
  }
}
