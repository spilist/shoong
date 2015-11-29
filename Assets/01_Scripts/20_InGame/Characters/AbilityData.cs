using UnityEngine;
using System.Collections;

namespace AbilityData {

  public enum BGM {
    PixelDance,
    JourneyAwaits,
    TrialOfSpikes,
    FullOfStars,
    DeepInTheCaves,
    Level_1,
    Level_2,
  }

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
    Laser = 4,
    Ghost = 5,
    Shield = 6,
    Polymorph = 8,
    Magnet = 16,
    Monster = 32,
    Metal = 64,
    Blink = 128,
    Gold = 256
  }
}
