using UnityEngine;
using System.Collections;
using AbilityData;

public class CharacterStat : MonoBehaviour {
  public string characterName;
  public string BGM;
  public Rarity rarity;
  public BaseSpeed baseSpeed;
  public BoosterPlusSpeed boosterPlusSpeed;
  public BoosterMaxSpeed boosterMaxSpeed;
  public BoosterSpeedDecrease boosterSpeedDecrease;
  public EnergyReduceOnTime energyReduceOnTime;
  public MaxEnergy maxEnergy;
  public ReboundDistance reboundDistance;
  public DamageGet damageGet;

  // [BitMask(typeof(SkillFlag))]
  public SkillFlag skillFlags;

  public float price() {
    return CharacterManager.cm.dolorsPerRarity[(int)rarity];
  }

  public string skillName() {
    string name = skillFlags.ToString();
    return name == "0" ? "" : name;
  }
}

public class BitMaskAttribute : PropertyAttribute {
  public System.Type propType;

  public BitMaskAttribute(System.Type aType) {
    propType = aType;
  }
}
