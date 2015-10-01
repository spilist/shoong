using UnityEngine;
using System.Collections;

public class CharacterCreateBannerButton : BannerButton {
  public CharacterCreateMenu createMenu;
  public GameObject menusOverlay;

  override public void activateSelf() {

    menusOverlay.SetActive(true);
    back.useBannerMenu(this, gameOverUI, createMenu.gameObject);
  }

  override public bool available() {
    return DataManager.dm.getInt("CurrentCubes") >= createMenu.price();
  }

  override public void goBack() {
    menusOverlay.SetActive(false);

    if (!available()) transform.parent.parent.gameObject.SetActive(false);
  }
}
