using UnityEngine;
using System.Collections;

public class ChangeControlButton : MenusBehavior {
	public string controlMethod;

  void OnEnable() {
    check();
  }

  override public void activateSelf() {
    DataManager.dm.setString("ControlMethod", controlMethod);
    filter.sharedMesh = activeMesh;

    foreach (Transform tr in transform.parent) {
      if (tr.tag == tag) tr.GetComponent<ChangeControlButton>().check();
    }
  }

  public void check() {
    if (DataManager.dm.getString("ControlMethod") != controlMethod) {
      filter.sharedMesh = inactiveMesh;
    }
  }
}
