using UnityEngine;
using System.Collections;

public class MetalManager : ObjectsManager {
  public float[] destroyBonusPerLevel;
  public float bonus;

  override public void initRest() {
  }

  override public void adjustForLevel(int level) {
    bonus = destroyBonusPerLevel[level - 1];
  }
}
