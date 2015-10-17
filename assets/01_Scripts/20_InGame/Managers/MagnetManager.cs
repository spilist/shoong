using UnityEngine;
using System.Collections;

public class MagnetManager : ObjectsManager {
  public float pullPushDuration = 3;
  public float pauseDuration = 0.5f;

  public int powerToParts = 50;
  public int powerToPlayer_pull = 80;
  public int powerToPlayer_push = 40;

  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

  override public void initRest() {
    int level = DataManager.dm.getInt("MagnetLevel") - 1;

    objEncounterEffectForPlayer.transform.localScale = Vector3.one * radiusPerLevel[level] / radiusPerLevel[0];
  }

}
