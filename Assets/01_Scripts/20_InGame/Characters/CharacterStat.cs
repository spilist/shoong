using UnityEngine;
using System.Collections;
using AbilityData;
using SmartLocalization;

public class CharacterStat : MonoBehaviour {
  public string characterName;
  public string skillName;
  public BGM bgm;
  public Rarity rarity;
  public BaseSpeed baseSpeed;
  public BoosterPlusSpeed boosterPlusSpeed;
  public BoosterMaxSpeed boosterMaxSpeed;
  public BoosterSpeedDecrease boosterSpeedDecrease;
  public EnergyReduceOnTime energyReduceOnTime;
  public MaxEnergy maxEnergy;
  public ReboundDistance reboundDistance;
  public DamageGet damageGet;
  public int bestX;
  public bool buyable = true;

  // [BitMask(typeof(SkillFlag))]
  public SkillFlag skillFlags;

  public string skillCode() {
    string name = skillFlags.ToString();
    return name == "0" ? "" : name;
  }

  void Start() {
    LanguageManager languageManager = LanguageManager.Instance;
    languageManager.OnChangeLanguage += OnChangeLanguage;
    OnChangeLanguage(languageManager);
  }

  void OnDestroy() {
    if(LanguageManager.HasInstance) {
      LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
    }
  }

  void OnChangeLanguage(LanguageManager languageManager) {
    characterName = LanguageManager.Instance.GetTextValue("CharacterName_" + name);
    skillName = skillCode()=="" ? "" : LanguageManager.Instance.GetTextValue("SkillName_" + skillCode());
  }
}

public class BitMaskAttribute : PropertyAttribute {
  public System.Type propType;

  public BitMaskAttribute(System.Type aType) {
    propType = aType;
  }
}
