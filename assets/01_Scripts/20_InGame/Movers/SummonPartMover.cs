using UnityEngine;
using System.Collections;

public class SummonPartMover : ObjectsMover {
  SummonPartsManager summonManager;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
    canBeMagnetized = false;
  }

  override protected void afterEncounter() {
    // sound
    summonManager.startSummon();
  }

  override public string getManager() {
    return "SummonPartsManager";
  }
}
