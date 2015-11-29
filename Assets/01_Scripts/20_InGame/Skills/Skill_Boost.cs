using UnityEngine;
using System.Collections;

public class Skill_Boost : Skill {
  public float speedUp = 2;

  override public void afterActivate(bool val) {
    if (val) Player.pl.setSpeedBoost(speedUp, duration);
  }
}
