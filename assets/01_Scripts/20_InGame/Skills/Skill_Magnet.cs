using UnityEngine;
using System.Collections;

public class Skill_Magnet : Skill {
  private float radiusScale = 1;

  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

	override public void adjustForLevel(int level) {
    useSkillEffect.transform.localScale = Vector3.one * radiusScale * radiusPerLevel[level] / radiusPerLevel[0];
  }

  public void moreRadius(int val) {
    radiusScale *= (100 + val) / 100f;
    adjustForLevel(level);
  }

  override public void resetAbility() {
    base.resetAbility();
    radiusScale = 1;
    adjustForLevel(level);
  }

  public float getRadiusScale() {
    return radiusScale;
  }
}
