using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  bool isGoldenCube = false;
  bool isSuperheatPart = false;
  ComboPartsManager cpm;

  public void setGolden() {
    isGoldenCube = true;
    objectsManager.objDestroyEffect = cpm.goldCubeDestroyParticle;
  }

  public void setSuper() {
    isSuperheatPart = true;
  }

  override protected void initializeRest() {
    cpm = (ComboPartsManager)objectsManager;
  }

  override public string getManager() {
    return "ComboPartsManager";
  }

  override protected void afterDestroy(bool byPlayer) {
    Destroy(cpm.nextInstance);
  }

  override protected bool beforeEncounter() {
    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cpm.pitchStart + cpm.getComboCount() * cpm.pitchIncrease;

    return true;
  }

  override protected void afterEncounter() {
    if (isGoldenCube) {
      cpm.gcCount.add(cpm.goldCubesGet);
    } else if (isSuperheatPart) {
      cpm.shm.add();
    } else {
      cpm.ptb.checkCollected(GetComponent<MeshFilter>().sharedMesh);
    }
    cpm.eatenByPlayer();
  }

  override public int cubesWhenDestroy() {
    int count = 0;
    for (int i = cpm.comboCount; i < cpm.fullComboCount; i++) {
      count += (i + 1);
    }

    return count * cpm.cubesByEncounter;
  }

  override public int bonusCubes() {
    return player.isNearAsteroid()? player.nearAsteroidBonus : 0;
  }
}
