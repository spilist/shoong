using UnityEngine;
using System.Collections;

public class InsideMenusBehavior : MenusBehavior {
  private MenusController menus;
  private BackButton back;

  void Start() {
    menus = GameObject.Find("Menus").GetComponent<MenusController>();
    back = menus.backButton.GetComponent<BackButton>();
  }

  override public void activateSelf() {
    GameObject newCurrent = menus.transform.Find(tag).gameObject;
    back.goInside(menus.currentMenu(), newCurrent);
  }
}
