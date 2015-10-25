using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Monster : Skill {
  public GameObject monsterFilter;
  public MonsterManager mm;

  override public void afterEnable() {
    mm.enabled = true;
  }

  override public void afterActivate(bool val) {
    EnergyManager.em.getFullHealth();

    monsterFilter.SetActive(val);

    if (!val) {
      transform.localScale = Player.pl.originalScale * Vector3.one;
      Player.pl.afterStrengthenStart();
      foreach (GameObject mm in GameObject.FindGameObjectsWithTag("MiniMonster")) {
        mm.GetComponent<MiniMonsterMover>().destroyObject();
      }
    }
  }
}
