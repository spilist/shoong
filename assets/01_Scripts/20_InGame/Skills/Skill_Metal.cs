using UnityEngine;
using System.Collections;

public class Skill_Metal : Skill {
  public float[] destroyBonusPerLevel;
  public float bonus;

  override public void adjustForLevel(int level) {
    bonus = destroyBonusPerLevel[level];
  }

  override public void afterActivate(bool val) {
    if (!val) Player.pl.afterStrengthenStart();
  }

}
