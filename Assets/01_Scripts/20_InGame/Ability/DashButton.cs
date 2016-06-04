using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashButton : MenusBehavior {
  private bool available = false;

  override public void activateSelf() {
    if (DashManager.dm.available()) DashManager.dm.smash();
  }

  void OnPointerDown() {
    activateSelf();
  }
}
