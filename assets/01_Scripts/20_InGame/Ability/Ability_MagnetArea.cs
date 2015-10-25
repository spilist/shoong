using UnityEngine;
using System.Collections;

public class Ability_MagnetArea : CharacterAbility {
  public Skill_Magnet mm;
  public int moreArea;

  override public void apply() {
    mm.moreRadius(moreArea);
  }
}
