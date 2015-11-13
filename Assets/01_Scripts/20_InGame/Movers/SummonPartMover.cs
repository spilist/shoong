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

  override public int cubesWhenDestroy() {
    return summonManager.numSpawnX * summonManager.numSpawnZ * 5;
  }

  override public int energyGets() {
    return 0;
  }
}
