using UnityEngine;
using System.Collections;

public class Ability_LessDamage : CharacterAbility {
  public int lessDamage;

	override public void apply() {
    EnergyManager.em.lessDamage(lessDamage);
  }
}
