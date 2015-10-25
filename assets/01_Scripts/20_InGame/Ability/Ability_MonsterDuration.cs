using UnityEngine;
using System.Collections;

public class Ability_MonsterDuration : CharacterAbility {
  public Skill_Monster skill_monster;
  public int moreDuration;

  override public void apply() {
    skill_monster.moreDuration(moreDuration);
  }
}
