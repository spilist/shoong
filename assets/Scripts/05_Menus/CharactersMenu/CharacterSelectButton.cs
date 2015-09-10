using UnityEngine;
using System.Collections;

public class CharacterSelectButton : MenusBehavior {
  public MenusController menus;
  public PlayerMover player;
  public PlayAgainButton playAgain;
  private string characterName;

  public void setCharacter(string val) {
    characterName = val;
  }

  override public void activateSelf() {
    PlayerPrefs.SetString("SelectedCharacter", characterName);
    player.changeCharacter(characterName);

    // if using banner, same as play again button
    if (menus.isMenuOn()) {
      menus.toggleMenuAndUI();
    } else {
      playAgain.activateSelf();
    }
  }
}
