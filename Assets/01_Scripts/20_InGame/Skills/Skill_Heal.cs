﻿using UnityEngine;
using System.Collections;

public class Skill_Heal : Skill {
  public int amount = 20;

  void Start() {
    if (DataManager.dm.isBonusStage) amount /= 2;
  }

  override public void afterActivate(bool val) {
    if (val) EnergyManager.em.getEnergy(amount);
  }
}
