using UnityEngine;
using System.Collections;

public class CharacterSelectButton : MenusBehavior {
  public MenusController menus;
  public CharacterChangeManager changeManager;
  public PlayAgainButton playAgain;
  private string characterName;

  public void setCharacter(string val) {
    characterName = val;
  }

  override public void activateSelf() {
    PlayerPrefs.SetString("SelectedCharacter", characterName);
    CharacterManager.cm.changeCharacter(characterName);

    // if using banner, same as play again button
    if (menus.isMenuOn()) {
      menus.toggleMenuAndUI();
    } else {
      playAgain.activateSelf();
    }
  }
}
