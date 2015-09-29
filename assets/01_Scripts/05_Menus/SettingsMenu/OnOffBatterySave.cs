using UnityEngine;
using System.Collections;

public class OnOffBatterySave : OnOffButton {
  public int batterySavingFrameRate = 25;

  override public void activateSelf() {
    base.activateSelf();

    if (clicked) {
      Application.targetFrameRate = batterySavingFrameRate;
      DataManager.dm.setInt(settingName + "Setting", batterySavingFrameRate);
    } else {
      Application.targetFrameRate = DataManager.dm.normalFrameRate;
      DataManager.dm.setInt(settingName + "Setting", DataManager.dm.normalFrameRate);
    }
  }
}
