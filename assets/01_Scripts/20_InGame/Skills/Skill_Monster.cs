using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Monster : Skill {
  public GameObject monsterFilter;
  public MonsterManager mm;

  override public void afterStart() {
    mm.enabled = true;
  }

  override public void afterActivate(bool val) {
    EnergyManager.em.getFullHealth();

    monsterFilter.SetActive(val);

    if (!val) {
      Player.pl.scaleBackByMonster();
      Player.pl.afterStrengthenStart();
      foreach (GameObject mm in GameObject.FindGameObjectsWithTag("MiniMonster")) {
        mm.GetComponent<MiniMonsterMover>().destroyObject();
      }
    }
  }
}
