using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsCategoryButton : MenusBehavior {
  public bool hasSelection = true;
  private ObjectsMenu objectsMenu;
  private string category;
  private Text objSelectionCount;
  private Text objSelectionLimit;

  void OnEnable() {
    category = name.Replace("Button", "");

    if (hasSelection) {
      objSelectionCount = transform.Find("SelectionCount").GetComponent<Text>();
      objSelectionLimit = transform.Find("SelectionLimit").GetComponent<Text>();
      checkSelection();
    }
  }

  override public void activateSelf() {
    objectsMenu = transform.parent.GetComponent<ObjectsMenu>();

    transform.Find("Text").GetComponent<Text>().color = objectsMenu.activeColor;
    if (hasSelection) {
      objSelectionCount.color = objectsMenu.activeColor;
      objSelectionLimit.color = objectsMenu.activeColor;
    }

    objectsMenu.resetAll(category);

    if (hasSelection) checkSelection();
  }

  public void changeSelectionCount(int amount) {
    int current = int.Parse(objSelectionCount.text);
    objSelectionCount.text = (current + amount).ToString();
  }

  void checkSelection() {
    string selectedObjectString = PlayerPrefs.GetString(category).Trim();
    if (selectedObjectString == "") return;

    string[] objs = selectedObjectString.Split(' ');
    foreach (string obj in objs) {
      transform.parent.Find(category + "/" + obj + "/ActiveBox").gameObject.SetActive(true);
    }
    objSelectionCount.text = objs.Length.ToString();
  }
}
