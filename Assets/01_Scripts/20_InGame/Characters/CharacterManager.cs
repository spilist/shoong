using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterManager : MonoBehaviour {
  public static CharacterManager cm;
  public CharacterChangeManager ccm;
  public MeshFilter progressCharacter;

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
  public float energyReduceOnTimeStandard_hard;
  private float original_energyReduceOnTimeStandard;
  private float original_energyReduceOnTimeStandard_hard;
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

  private string currentCharacter;
  public bool isRandom;

  private List<string> characterNames;
  private List<string> randomList;

  void Awake() {
    cm = this;

    original_baseSpeedStandard = baseSpeedStandard;
    original_boosterPlusSpeedStandard = boosterPlusSpeedStandard;
    original_boosterMaxSpeedStandard = boosterMaxSpeedStandard;
    original_boosterSpeedDecreaseStandard = boosterSpeedDecreaseStandard;
    original_energyReduceOnTimeStandard = energyReduceOnTimeStandard;
    original_energyReduceOnTimeStandard_hard = energyReduceOnTimeStandard_hard;
    original_maxEnergyStandard = maxEnergyStandard;
    original_reboundTimeScaleStandard = reboundTimeScaleStandard;
    original_damageGetScaleStandard = damageGetScaleStandard;

    characterNames = new List<string>();
    Transform characters = transform.Find("Characters");
    for (int i = 0; i < characters.childCount; i++) {
      characterNames.Add(characters.GetChild(i).name);
    }
    randomList = new List<string>();
  }

  void resetToOrginal() {
    baseSpeedStandard = original_baseSpeedStandard;
    boosterPlusSpeedStandard = original_boosterPlusSpeedStandard;
    boosterMaxSpeedStandard = original_boosterMaxSpeedStandard;
    boosterSpeedDecreaseStandard = original_boosterSpeedDecreaseStandard;
    energyReduceOnTimeStandard = original_energyReduceOnTimeStandard;
    energyReduceOnTimeStandard_hard = original_energyReduceOnTimeStandard_hard;
    maxEnergyStandard = original_maxEnergyStandard;
    reboundTimeScaleStandard = original_reboundTimeScaleStandard;
    damageGetScaleStandard = original_damageGetScaleStandard;
  }

  public CharacterStat character(string name) {
    return transform.Find("Characters/" + name).GetComponent<CharacterStat>();
  }

  public void startRandom(bool val = true) {
    isRandom = val;
    if (val) StartCoroutine("randomCharacter");
    else StopCoroutine("randomCharacter");
  }

  IEnumerator randomCharacter() {
    randomList = shuffle(characterNames);

    int index = 0;
    while(true) {
      setMesh(randomList[index]);
      yield return new WaitForSeconds(0.15f);
      index++;
      if (index >= randomList.Count) index = 0;
    }
  }

  List<string> shuffle(List<string> list) {
    int n = list.Count;
    System.Random rnd = new System.Random();
    while (n > 1) {
        int k = (rnd.Next(0, n) % n);
        n--;
        string value = list[k];
        list[k] = list[n];
        list[n] = value;
    }

    return list;
  }

  public void setMesh(string name) {
    ccm.setMesh(character(name).GetComponent<MeshFilter>().sharedMesh);
  }

  public void startGame() {
    if (isRandom) {
      StopCoroutine("randomCharacter");
      changeCharacter(Player.pl.GetComponent<MeshFilter>().sharedMesh.name);
      DataManager.dm.increment("RandomPlayAvailable", -1);
      DataManager.dm.setDateTime("LastRandomPlayTime");
      DataManager.dm.save();
    }
  }

  public void changeCharacter(string name) {
    currentCharacter = name;
    CharacterStat stat = character(name);
    ccm.setMesh(stat.GetComponent<MeshFilter>().sharedMesh);
    if (progressCharacter != null) {
      progressCharacter.sharedMesh = stat.GetComponent<MeshFilter>().sharedMesh;
    }
    AudioManager.am.setAudio((int) stat.bgm);
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

    Player.pl.resetAbility(stat.bestX);

    energyReduceOnTimeStandard *= 1 + scale_energyReduceOnTimes;
    energyReduceOnTimeStandard_hard *= 1 + scale_energyReduceOnTimes;
    maxEnergyStandard *= 1 + scale_maxEnergys;
    damageGetScaleStandard *= 1 + scale_damageGets;

    EnergyManager.em.resetAbility();

    string skillName = stat.skillFlags.ToString();
    if (skillName == "0") {
      RhythmManager.rm.setLoop(0, 0);
      SkillManager.sm.equip("None");
    } else {
      SkillManager.sm.equip(skillName);
    }
  }

  public string getCurrentCharacter() {
    return currentCharacter;
  }
}
