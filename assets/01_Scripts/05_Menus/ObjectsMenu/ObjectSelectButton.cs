using UnityEngine;
using System.Collections;

public class ObjectSelectButton : MenusBehavior {
  public Mesh originalMesh;
  public Mesh reachedLimitMesh;

  public AudioClip errorSound;
  private UIObjects selectedObj;
  private string objName;
  private string category;
  private MeshFilter filter;

  void Awake() {
    playTouchSound = false;
    filter = GetComponent<MeshFilter>();
  }

  public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objName = obj.transform.parent.name;
    category = obj.transform.parent.parent.name;

    if (limitReached()) {
      filter.sharedMesh = reachedLimitMesh;
    } else {
      filter.sharedMesh = originalMesh;
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
