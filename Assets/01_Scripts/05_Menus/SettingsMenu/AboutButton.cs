﻿using UnityEngine;
using System.Collections;

public class AboutButton : InsideMenusBehavior {
  public CharactersMenu cm;
  public bool cheat;

  override public void afterActivate() {
    if (cheat) cm.allCharacters();
  }
}
