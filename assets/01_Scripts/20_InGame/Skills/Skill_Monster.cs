using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Monster : Skill {
  public GameObject monsterFilter;
  public MonsterManager mm;
  private CharacterChangeManager cm;

  override public void afterStart() {
    mm.enabled = true;
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

  override public void afterActivate(bool val) {
    monsterFilter.SetActive(val);

    if (val) {
      cm.changeCharacterTo("Monster");
    } else {
      cm.changeCharacterToOriginal();
      Player.pl.scaleBackByMonster();
      Player.pl.afterStrengthenStart();
      foreach (GameObject mm in GameObject.FindGameObjectsWithTag("MiniMonster")) {
        mm.GetComponent<MiniMonsterMover>().destroyObject();
      }
    }
  }
}
