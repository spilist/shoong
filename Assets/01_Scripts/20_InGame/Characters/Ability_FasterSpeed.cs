using UnityEngine;
using System.Collections;

public class Ability_FasterSpeed : CharacterAbility {
  public int moreSpeedRate;

  override public void apply() {
    Player.pl.fasterSpeed(moreSpeedRate);
  }
}
