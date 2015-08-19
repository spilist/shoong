using UnityEngine;
using System.Collections;

public class BackButton : MenusBehavior {
  public MenusController menus;

  override public void activateSelf() {
    menus.toggleMenuAndUI();
	}
}
