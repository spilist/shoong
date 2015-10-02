using UnityEngine;
using System.Collections;

public class CharacterCreateBannerButton : BannerButton {
  public CharacterCreateMenu createMenu;
  public GameObject menusOverlay;
  public int showMoreUntil = 2;

  override public void activateSelf() {
    menusOverlay.SetActive(true);
    back.useBannerMenu(this, gameOverUI, createMenu.gameObject);
    DataManager.dm.setInt("ShowCharacterCreateCount", 0);
  }

  override public bool available(int spaceLeft) {
    if (DataManager.dm.getInt("ShowCharacterCreateCount") > showMoreUntil) return false;

    if (DataManager.dm.getInt("CurrentCubes") >= createMenu.price()) {
      DataManager.dm.increment("ShowCharacterCreateCount");
      return true;
    }

    return false;
  }

  override public void goBack() {
    menusOverlay.SetActive(false);

    if (!available(1)) transform.parent.parent.gameObject.SetActive(false);
  }
}
