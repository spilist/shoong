using UnityEngine;
using System.Collections;

public class SummonPartMover : ObjectsMover {
  SummonPartsManager summonManager;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
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
    Destroy(gameObject);

    // encouter effect
    // showEffect("Summon")
    summonManager.startSummon();
  }

  override public string getManager() {
    return "SummonPartsManager";
  }
}
