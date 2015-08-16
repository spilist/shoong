using UnityEngine;
using System.Collections;

public class CharacterSelectButton : MenusBehavior {
  public MenusController menus;
  public PlayerMover player;
  private string characterName;

  public void setCharacter(string val) {
    characterName = val;
  }

  override public void activateSelf() {
    PlayerPrefs.SetString("SelectedCharacter", characterName);
    menus.toggleMenuAndUI();
    player.changeCharacter(characterName);
  }
}
