using UnityEngine;
using System.Collections;

public class Skill_Metal : Skill {
  private CharacterChangeManager cm;

  override public void afterStart() {
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

  override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Metal");
    } else {
      cm.changeCharacterToOriginal();
      Player.pl.afterStrengthenStart();
    }
  }
}
