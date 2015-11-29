using UnityEngine;
using System.Collections;

public class JetpackManager : ObjectsManager {
  public float[] boosterBonusScalePerLevel;
  public float boosterBonusScale;

  public int minBoosterAmount = 40;
  public int maxBoosterAmonut = 120;
  public int boosterSpeedDecreaseBase = 70;
  public int boosterSpeedDecreasePerTime = 20;

  public float delayAfterMove = 0.5f;

  override public void initRest() {
    int level = DataManager.dm.getInt("JetpackLevel") - 1;

    boosterBonusScale = boosterBonusScalePerLevel[level];
  }
}
