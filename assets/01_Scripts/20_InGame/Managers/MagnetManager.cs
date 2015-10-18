using UnityEngine;
using System.Collections;

public class MagnetManager : ObjectsManager {
  public int power = 50;

  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

  override public void initRest() {
    int level = DataManager.dm.getInt("MagnetLevel") - 1;

    objEncounterEffectForPlayer.transform.localScale = Vector3.one * radiusPerLevel[level] / radiusPerLevel[0];
  }
}
