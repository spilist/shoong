using UnityEngine;
using System.Collections;

public class SummonPartMover : ObjectsMover {
  SummonPartsManager summonManager;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
  }

  override protected void afterEncounter() {
    summonManager.startSummon();
  }

  override public string getManager() {
    return "SummonPartsManager";
  }
}
