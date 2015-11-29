using UnityEngine;
using System.Collections;

public class Skill_Blink : Skill {
  public DoppleManager dpm;
  private CharacterChangeManager cm;

  override public void afterStart() {
    dpm.enabled = true;
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

	override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Blink");
    } else {
      cm.changeCharacterToOriginal();
    }
  }
}
