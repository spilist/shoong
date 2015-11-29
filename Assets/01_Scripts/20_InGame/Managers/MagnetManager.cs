using UnityEngine;
using System.Collections;

public class MagnetManager : ObjectsManager {
  public int power = 50;

  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

  override public void initRest() {
  }

  override public void adjustForLevel(int level) {
    objEncounterEffectForPlayer.transform.localScale = Vector3.one * radiusPerLevel[level - 1] / radiusPerLevel[0];
  }
}
