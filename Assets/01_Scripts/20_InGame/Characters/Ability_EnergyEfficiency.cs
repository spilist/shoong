using UnityEngine;
using System.Collections;

public class Ability_EnergyEfficiency : CharacterAbility {
  public int efficiency;

  override public void apply() {
    EnergyManager.em.moreEnergyEfficiency(efficiency);
  }
}
