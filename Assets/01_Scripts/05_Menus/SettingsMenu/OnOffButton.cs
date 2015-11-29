using UnityEngine;
using System.Collections;

public class OnOffButton : MenusBehavior {
  protected bool clicked = false;

  public string settingName;

  void Start() {
    clicked = DataManager.dm.getBool(settingName + "Setting");
    applyStatus();
  }

  override public void activateSelf() {
    clicked = !clicked;
    DataManager.dm.setBool(settingName + "Setting", clicked);
    applyStatus();
    DataManager.dm.save();
  }

  virtual public void applyStatus() {
    if (clicked) {
      filter.sharedMesh = inactiveMesh;
    } else {
      filter.sharedMesh = activeMesh;
    }
  }
}
