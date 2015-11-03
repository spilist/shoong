using UnityEngine;
using System.Collections;

public class Skill_Ghost : Skill {
  public Collider contactCollider;
  private CharacterChangeManager cm;

  override public void afterStart() {
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

  override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Ghost");
    } else {
      cm.changeCharacterToOriginal();
    }

    contactCollider.enabled = !val;

    // 속도 증가
  }
}
