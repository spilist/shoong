﻿using UnityEngine;
using System.Collections;

public class Skill_Metal : Skill {
  public float scaleUpAmount = 0.2f;
  private CharacterChangeManager cm;

  override public void afterStart() {
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

  override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Metal");
      // Player.pl.scaleUp(1 + scaleUpAmount);
    } else {
      cm.changeCharacterToOriginal();
      Player.pl.afterStrengthenStart();
    }
  }
}
