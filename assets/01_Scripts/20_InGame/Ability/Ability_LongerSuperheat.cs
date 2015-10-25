using UnityEngine;
using System.Collections;

public class Ability_LongerSuperheat : CharacterAbility {
  public Superheat superheat;
  public int howLonger;

  override public void apply() {
    superheat.longerSuperheat(howLonger);
  }
}
