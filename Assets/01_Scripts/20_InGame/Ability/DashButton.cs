using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashButton : MenusBehavior {
  private bool available = false;

  override public void activateSelf() {
    if (available) {
      Player.pl.dash();
    }
  }

  public void enableAbility() {
    available = true;
    filter.sharedMesh = activeMesh;
    playTouchSound = true;
  }

  void disableAbility() {
    available = false;
    filter.sharedMesh = inactiveMesh;
    playTouchSound = false;
  }

  void OnPointerDown() {
    activateSelf();
  }
}
