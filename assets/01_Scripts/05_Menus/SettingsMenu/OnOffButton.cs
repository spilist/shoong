using UnityEngine;
using System.Collections;

public class OnOffButton : MenusBehavior {
  protected bool clicked = false;
  private MeshFilter filter;

  public string settingName;
  public Mesh onMesh;
  public Mesh offMesh;

  void Start() {
    filter = GetComponent<MeshFilter>();
    clicked = DataManager.dm.getBool(settingName + "Setting");
    applyStatus();
  }

  override public void activateSelf() {
    clicked = !clicked;
    DataManager.dm.setBool(settingName + "Setting", clicked);
    applyStatus();
  }

  void applyStatus() {
    if (clicked) {
      filter.sharedMesh = offMesh;
    } else {
      filter.sharedMesh = onMesh;
    }
  }
}
