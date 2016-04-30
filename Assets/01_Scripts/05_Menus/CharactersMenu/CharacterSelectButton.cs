using UnityEngine;
using System.Collections;

public class CharacterSelectButton : MenusBehavior {
  public MenusController menus;
  public CharacterChangeManager changeManager;
  public PlayAgainButton playAgain;
  public bool isBonusStage;
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
      DataManager.dm.isBonusStage = isBonusStage;
      playAgain.stopResetBonus();
      playAgain.activateSelf();
    }
  }
}
