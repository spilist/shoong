using UnityEngine;
using System.Collections;

public class Skill_Heal : Skill {
  int amount = 20;

  override public void afterActivate(bool val) {
    if (val) EnergyManager.em.getEnergy(amount);
  }
}
