using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashButton : MenusBehavior {
  private bool available = false;

  override public void activateSelf() {
    // Using audiolistener to check game is paused is just a hack
    if (DashManager.dm.available() && Player.pl.paused == false) {
      DashManager.dm.smash(true);
    }
  }

  void OnPointerDown() {
    activateSelf();
  }
}
