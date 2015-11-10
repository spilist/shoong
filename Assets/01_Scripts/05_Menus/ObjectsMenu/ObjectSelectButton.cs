using UnityEngine;
using System.Collections;

public class ObjectSelectButton : MenusBehavior {
  public Transform objectsMenu;
  public AudioClip errorSound;
  private UIObjects selectedObj;
  private string objName;
  private string category;
  private int limit;

  public void setObject(UIObjects obj) {
    if (!obj.isCollector) gameObject.SetActive(true);
    selectedObj = obj;
    objName = obj.transform.parent.name;
    category = obj.transform.parent.parent.name;

    limit = objectsMenu.Find(category + "Button").GetComponent<ObjectsCategoryButton>().selectionLimit;

    if (limitReached()) {
      filter.sharedMesh = inactiveMesh;
    } else {
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
    return PlayerPrefs.GetString(category).Trim().Split(' ').Length >= limit;
  }
}
