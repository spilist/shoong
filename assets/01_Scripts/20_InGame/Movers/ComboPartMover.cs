using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  ComboPartsManager cpm;

  override protected void initializeRest() {
    cpm = (ComboPartsManager)objectsManager;
    canBeMagnetized = false;
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
    cpm.eatenByPlayer();
  }
}
