using UnityEngine;
using System.Collections;

public class ChangeDashButton : MenusBehavior {
  public string dashMode;

  void OnEnable() {
    check();
  }

  override public void activateSelf() {
    DataManager.dm.setString("DashMode", dashMode);
    filter.sharedMesh = activeMesh;

    foreach (Transform tr in transform.parent) {
      ChangeDashButton cdb = tr.GetComponent<ChangeDashButton>();
      if (cdb != null) cdb.check();
    }
  }

  public void check() {
    if (DataManager.dm.getString("DashMode") != dashMode) {
      filter.sharedMesh = inactiveMesh;
    }
  }
}
