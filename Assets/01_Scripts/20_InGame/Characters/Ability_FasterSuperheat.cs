using UnityEngine;
using System.Collections;

public class Ability_FasterSuperheat : CharacterAbility {
  public int moreSuperheatRate;

  override public void apply() {
    SuperheatManager.sm.fasterSuperheat(moreSuperheatRate);
  }
}
