﻿using UnityEngine;
using System.Collections;

public class Skill_Metal : Skill {
  override public void afterActivate(bool val) {
    if (!val) Player.pl.afterStrengthenStart();
  }
}
