using UnityEngine;
using System.Collections;

public class ObjectSelectButton : MenusBehavior {
  public AudioClip errorSound;
  private UIObjects selectedObj;
  private string objName;
  private string category;

  public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objName = obj.transform.parent.name;
    category = obj.transform.parent.parent.name;

    if (limitReached()) {
      filter.sharedMesh = inactiveMesh;
    } else {
      Debug.Log(filter);
      filter.sharedMesh = activeMesh;
    }
  }

  override public void activateSelf() {
    if (limitReached()) {
      AudioSource.PlayClipAtPoint(errorSound, transform.position);
    }
    else {
      string selectedObjString = PlayerPrefs.GetString(category).Trim();

      PlayerPrefs.SetString(category, (selectedObjString + " " + objName).Trim());
      selectedObj.setActive(true);
    }
  }

  public bool limitReached() {
    return PlayerPrefs.GetString(category).Trim().Split(' ').Length >= 3;
  }
}
