using UnityEngine;
using System.Collections;

public class BackButton : MenusBehavior {
  public MenusController menus;

  private GameObject prev;
  private GameObject current;

  private bool inside = false;
  private bool usingBannerMenu = false;
  private BannerButton banner;

  override public void activateSelf() {
    if (inside) {
      inside = false;
      prev.SetActive(true);
      current.SetActive(false);
      menus.setCurrentMenu(prev);
    } else if (usingBannerMenu) {
      usingBannerMenu = false;
      gameObject.SetActive(false);
      prev.SetActive(true);
      current.SetActive(false);
      banner.goBack();
    } else {
      menus.toggleMenuAndUI();
    }
	}

  public void goInside(GameObject prev, GameObject current) {
    inside = true;
    this.prev = prev;
    this.current = current;

    prev.SetActive(false);
    current.SetActive(true);
    menus.setCurrentMenu(current);
  }

  public void useBannerMenu(BannerButton banner, GameObject prev, GameObject current) {
    usingBannerMenu = true;
    this.banner = banner;
    this.prev = prev;
    this.current = current;

    prev.SetActive(false);
    current.SetActive(true);
    gameObject.SetActive(true);
  }
}
