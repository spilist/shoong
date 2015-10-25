using UnityEngine;
using System.Collections;

public class Ability_FasterSuperheat : CharacterAbility {
  public Superheat superheat;
  public int moreSuperheatRate;

  override public void apply() {
    superheat.fasterSuperheat(moreSuperheatRate);
  }
}
