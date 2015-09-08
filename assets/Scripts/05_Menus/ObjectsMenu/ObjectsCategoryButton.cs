using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsCategoryButton : MenusBehavior {
  public GameObject another;
  private ObjectsMenu objectsMenu;
  private string category;
  private Text objSelectionCount;

  void OnEnable() {
    objSelectionCount = transform.Find("SelectionCount").GetComponent<Text>();
    category = name.Replace("Button", "");
    checkSelection();
  }

  override public void activateSelf() {
    objectsMenu = transform.parent.GetComponent<ObjectsMenu>();

    transform.Find("Text").GetComponent<Text>().color = objectsMenu.activeColor;
    objSelectionCount.color = objectsMenu.activeColor;
    transform.Find("SelectionLimit").GetComponent<Text>().color = objectsMenu.activeColor;

    another.transform.Find("Text").GetComponent<Text>().color = objectsMenu.inactiveColor;
    another.transform.Find("SelectionCount").GetComponent<Text>().color = objectsMenu.inactiveColor;
    another.transform.Find("SelectionLimit").GetComponent<Text>().color = objectsMenu.inactiveColor;

    objectsMenu.showEmptyDescription(category);
    transform.parent.Find(category).gameObject.SetActive(true);
    transform.parent.Find(another.name.Replace("Button", "")).gameObject.SetActive(false);

    objectsMenu.resetAll(category);
    checkSelection();
  }

  public void changeSelectionCount(int amount) {
    int current = int.Parse(objSelectionCount.text);
    objSelectionCount.text = (current + amount).ToString();
  }

  void checkSelection() {
    string selectedObjectString = DataManager.dm.getString(category);
    if (selectedObjectString == "") return;

    string[] objs = selectedObjectString.Split(' ');
    foreach (string obj in objs) {
      transform.parent.Find(category + "/" + obj + "/ActiveBox").gameObject.SetActive(true);
    }
    objSelectionCount.text = objs.Length.ToString();
  }
}
