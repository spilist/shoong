using UnityEngine;
using System.Collections;

public class ObjectSelectButton : MenusBehavior {
  private UIObjects selectedObj;
  private string objName;
  private string category;

  void Start() {
    playTouchSound = false;
  }

  public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objName = obj.transform.parent.name;
    category = obj.transform.parent.parent.name;
  }

  override public void activateSelf() {
    string selectedObjString = PlayerPrefs.GetString(category);
    if (selectedObjString.Split(' ').Length == 3) return;

    PlayerPrefs.SetString(category, (selectedObjString + " " + objName).Trim());
    selectedObj.setActive(true);
  }
}
