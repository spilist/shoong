using UnityEngine;
using System.Collections;

namespace AbilityData {

  public enum Rarity {
    Common,
    Rare,
    Epic,
    Legendary
  }

  public enum BaseSpeed {
    S, A, B, C, D
  }

  public enum BoosterPlusSpeed {
    S, A, B, C, D
  }

  public enum BoosterMaxSpeed {
    S, A, B, C, D
  }

  public enum BoosterSpeedDecrease {
    S, A, B, C, D
  }

  public enum EnergyReduceOnTime {
    S, A, B, C, D
  }

  public enum MaxEnergy {
    S, A, B, C, D
  }

  public enum ReboundDistance {
    S, A, B, C, D
  }

  public enum DamageGet {
    S, A, B, C, D
  }

  public enum SkillFlag {
    Heal = 1,
    Fever = 2,
    Boost = 4,
    Polymorph = 8,
    Magnet = 16,
    Monster = 32,
    Metal = 64,
    Blink = 128,
    Gold = 256
  }
}
