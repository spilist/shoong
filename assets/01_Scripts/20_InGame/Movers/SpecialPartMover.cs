using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
  }

  override public void destroyObject(bool destroyEffect = true) {
    Destroy(gameObject);

    if (destroyEffect) {
      // destroyEffect
    }

    objectsManager.skipRespawnInterval();
    objectsManager.run();
  }

  override public void encounterPlayer() {
    changeManager.getSpecialEnergyEffect.Play();
    player.startUnstoppable();
    Destroy(gameObject);
  }

  override public string getManager() {
    return "SpecialPartsManager";
  }
}
