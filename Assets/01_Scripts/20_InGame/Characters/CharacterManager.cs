using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
  public static CharacterManager cm;
  public CharacterChangeManager ccm;

  public float[] dolorsPerRarity;

  public float baseSpeedStandard;
  private float original_baseSpeedStandard;
  public int[] baseSpeeds;

  public float boosterPlusSpeedStandard;
  private float original_boosterPlusSpeedStandard;
  public int[] boosterPlusSpeeds;

  public float boosterMaxSpeedStandard;
  private float original_boosterMaxSpeedStandard;
  public int[] boosterMaxSpeeds;

  public float boosterSpeedDecreaseStandard;
  private float original_boosterSpeedDecreaseStandard;
  public int[] boosterSpeedDecreases;

  public float energyReduceOnTimeStandard;
  private float original_energyReduceOnTimeStandard;
  public int[] energyReduceOnTimes;

  public float maxEnergyStandard;
  private float original_maxEnergyStandard;
  public int[] maxEnergys;

  public float reboundTimeScaleStandard;
  private float original_reboundTimeScaleStandard;
  public int[] reboundDistances;

  public float damageGetScaleStandard;
  private float original_damageGetScaleStandard;
  public int[] damageGets;

  void Awake() {
    cm = this;

    original_baseSpeedStandard = baseSpeedStandard;
    original_boosterPlusSpeedStandard = boosterPlusSpeedStandard;
    original_boosterMaxSpeedStandard = boosterMaxSpeedStandard;
    original_boosterSpeedDecreaseStandard = boosterSpeedDecreaseStandard;
    original_energyReduceOnTimeStandard = energyReduceOnTimeStandard;
    original_maxEnergyStandard = maxEnergyStandard;
    original_reboundTimeScaleStandard = reboundTimeScaleStandard;
    original_damageGetScaleStandard = damageGetScaleStandard;
  }

  void resetToOrginal() {
    baseSpeedStandard = original_baseSpeedStandard;
    boosterPlusSpeedStandard = original_boosterPlusSpeedStandard;
    boosterMaxSpeedStandard = original_boosterMaxSpeedStandard;
    boosterSpeedDecreaseStandard = original_boosterSpeedDecreaseStandard;
    energyReduceOnTimeStandard = original_energyReduceOnTimeStandard;
    maxEnergyStandard = original_maxEnergyStandard;
    reboundTimeScaleStandard = original_reboundTimeScaleStandard;
    damageGetScaleStandard = original_damageGetScaleStandard;
  }

  public CharacterStat character(string name) {
    return transform.Find("Characters/" + name).GetComponent<CharacterStat>();
  }

  public void changeCharacter(string name) {
    CharacterStat stat = character(name);
    ccm.setMesh(stat.GetComponent<MeshFilter>().sharedMesh);
    AudioManager.am.setAudio(stat.BGM);
    resetToOrginal();

    float scale_baseSpeeds = baseSpeeds[(int)stat.baseSpeed] / 100.0f;
    float scale_boosterPlusSpeeds = boosterPlusSpeeds[(int)stat.boosterPlusSpeed] / 100.0f;
    float scale_boosterMaxSpeeds = boosterMaxSpeeds[(int)stat.boosterMaxSpeed] / 100.0f;
    float scale_boosterSpeedDecreases = boosterSpeedDecreases[(int)stat.boosterSpeedDecrease] / 100.0f;
    float scale_energyReduceOnTimes = energyReduceOnTimes[(int)stat.energyReduceOnTime] / 100.0f;
    float scale_maxEnergys = maxEnergys[(int)stat.maxEnergy] / 100.0f;
    float scale_damageGets = damageGets[(int)stat.damageGet] / 100.0f;
    float scale_reboundDistances = reboundDistances[(int)stat.reboundDistance] / 100.0f;

    baseSpeedStandard *= 1 + scale_baseSpeeds;
    boosterPlusSpeedStandard *= 1 + scale_boosterPlusSpeeds;
    boosterMaxSpeedStandard *= 1 + scale_boosterMaxSpeeds;
    boosterSpeedDecreaseStandard *= 1 + scale_boosterSpeedDecreases;
    reboundTimeScaleStandard *= 1 + scale_reboundDistances;

    Player.pl.resetAbility();

    energyReduceOnTimeStandard *= 1 + scale_energyReduceOnTimes;
    maxEnergyStandard *= 1 + scale_maxEnergys;
    damageGetScaleStandard *= 1 + scale_damageGets;

    EnergyManager.em.resetAbility();


    string skillName = stat.skillFlags.ToString();
    if (skillName == "0") {
      RhythmManager.rm.setLoop(0, 0);
    } else {
      SkillManager.sm.equip(skillName);
    }
  }
}
