using UnityEngine;
using System.Collections;

public class Ability_LongerSuperheat : CharacterAbility {
  public int howLonger;

  override public void apply() {
    SuperheatManager.sm.longerSuperheat(howLonger);
  }
}
