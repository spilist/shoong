using UnityEngine;
using System.Collections;

public class Skill_Magnet : Skill {
  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

	override public void adjustForLevel(int level) {
    useSkillEffect.transform.localScale = Vector3.one * radiusPerLevel[level] / radiusPerLevel[0];
  }
}
