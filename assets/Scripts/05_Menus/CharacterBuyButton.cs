using UnityEngine;
using System.Collections;

public class CharacterBuyButton : MenusBehavior {
  private string characterName;

  public void setCharacter(string val) {
    characterName = val;
  }
}
