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
    cpm.eatenByPlayer();
    player.getComboParts.Play();
    player.getComboParts.GetComponent<AudioSource>().Play ();
    Destroy(gameObject);
  }

  override public int cubesWhenEncounter() {
    return cpm.getComboCount() * cpm.comboBonusScale;
  }
}
