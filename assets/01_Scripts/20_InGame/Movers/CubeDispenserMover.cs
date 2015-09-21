using UnityEngine;
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
      destroyObject();
      player.setTrapped(false);
      return;
    }

    Camera.main.GetComponent<CameraMover>().shake();
    cdm.tryBreak();

    GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[brokenCount];
    brokenCount++;
  }

  override protected void afterDestroy() {
    player.setTrapped(false);
  }

  override protected void afterCollide(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      if (player.isUsingRainbow()) {
        player.goodPartsEncounter(this, cdm.cubesPerContact * cdm.fullComboCount);
      } else {
        player.contactCubeDispenser(transform, cdm.cubesPerContact, collision, cdm.reboundDuring);

        encounterPlayer(false);
      }
    }
  }

  override protected bool beforeEncounter() {
    cdm.checkTrying();

    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cdm.pitchStart + comboCount * cdm.pitchIncrease;

    GetComponent<ParticleSystem>().emissionRate -= cdm.decreaseEmissionAmount;

    comboCount++;

    if (comboCount == cdm.fullComboCountPerLevel[0]) {
      QuestManager.qm.addCountToQuest("CubeDispenser");
    }

    if (comboCount == cdm.fullComboCount - 2) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[0];

    if (comboCount == cdm.fullComboCount - 1) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[1];

    if (comboCount == cdm.fullComboCount) {
      QuestManager.qm.addCountToQuest("CompleteCubeDispenser");
      destroyObject();
      player.showEffect("Great");
      return false;
    }

    return true;
  }

  override public string getManager() {
    return "CubeDispenserManager";
  }

  override public int cubesWhenEncounter() {
    return cdm.cubesPerContact * cdm.fullComboCount;
  }
}
