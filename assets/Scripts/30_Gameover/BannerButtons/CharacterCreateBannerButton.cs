using UnityEngine;
using System.Collections;

public class CharacterCreateBannerButton : BannerButton {
  public CharacterCreateMenu createMenu;

  override public void activateSelf() {
    Debug.Log("character create menu show");
  }

  override public bool available() {
    return (int) GameController.control.cubes["now"] >= createMenu.createPrice;
  }
}
