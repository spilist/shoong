using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  ComboPartsManager cpm;

  override protected void initializeRest() {
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    canBeMagnetized = false;
  }

  override public void destroyObject(bool destroyEffect = true) {
    cpm.destroyInstances();
  }

  override public string getManager() {
    return "ComboPartsManager";
  }

  override public void encounterPlayer() {
    changeManager.getComboParts.Play();
    AudioSource getComboParts = changeManager.getComboParts.GetComponent<AudioSource>();
    getComboParts.pitch = cpm.pitchStart + cpm.getComboCount() * cpm.pitchIncrease;
    getComboParts.Play ();
    cpm.eatenByPlayer();
    Destroy(gameObject);
  }

  override public int cubesWhenEncounter() {
    return (cpm.getComboCount() + 1) * cpm.comboBonusScale;
  }
}
