using UnityEngine;
using System.Collections;

public class Ability_MetalSpeed : CharacterAbility {
  public Skill_Metal skill_metal;
  public int moreSpeed;

  override public void apply() {
    Player.pl.moreSpeedOn(moreSpeed, "Metal");
  }
}
