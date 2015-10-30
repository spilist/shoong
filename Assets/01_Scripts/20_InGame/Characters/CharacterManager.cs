using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
  public static CharacterManager cm;

  public int[] pricesPerRarity;

  void Awake() {
    cm = this;
  }

  public CharacterStat character(string name) {
    return transform.Find("Characters/" + name).GetComponent<CharacterStat>();
  }

  public void resetAllAbility() {
    // EnergyManager.em.resetEnergyAbility();
    // Player.pl.resetAbility();
    // CubeManager.cm.resetCubeAbility();
    // skill_magnet.resetAbility();
    // skill_monster.resetAbility();
    // skill_metal.resetAbility();
  }
}
