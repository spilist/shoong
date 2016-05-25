using UnityEngine;
using System.Collections;

public class Skill_Ghost : Skill {
  public Collider contactCollider;
  public GameObject ghostFilter;
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

    ghostFilter.SetActive(val);
    contactCollider.enabled = !val;
  }
}
