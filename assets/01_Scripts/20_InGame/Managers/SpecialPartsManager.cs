using UnityEngine;
using System.Collections;

public class SpecialPartsManager : ObjectsManager {
  public float[] destroyBonusPerLevel;
  public float bonus;

  override public void initRest() {
    bonus = destroyBonusPerLevel[DataManager.dm.getInt("SpecialPartsLevel") - 1];
  }
}
