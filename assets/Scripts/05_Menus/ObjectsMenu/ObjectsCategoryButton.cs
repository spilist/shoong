using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsCategoryButton : MenusBehavior {
  public GameObject another;
  private ObjectsMenu objectsMenu;
  private string category;
  private Text objSelectionCount;

  override public void activateSelf() {
    objectsMenu = transform.parent.GetComponent<ObjectsMenu>();

    transform.Find("Text").GetComponent<Text>().color = objectsMenu.activeColor;
    another.transform.Find("Text").GetComponent<Text>().color = objectsMenu.inactiveColor;

    category = name.Replace("Button", "");

    objectsMenu.showEmptyDescription(category);
    transform.parent.Find(category).gameObject.SetActive(true);
    transform.parent.Find(another.name.Replace("Button", "")).gameObject.SetActive(false);

    objSelectionCount = objectsMenu.transform.Find(category + "/ObjectsSelectionCount").GetComponent<Text>();

    objectsMenu.resetAll(category);
    string selectedObjectString = PlayerPrefs.GetString(category);
    if (selectedObjectString == "") return;

    string[] objs = selectedObjectString.Split(' ');
    foreach (string obj in objs) {
      transform.parent.Find(category + "/" + obj + "/ActiveBox").gameObject.SetActive(true);
    }
    objSelectionCount.text = objs.Length.ToString();
	}

  public void changeSelectionCount(int amount) {
    int current = int.Parse(objSelectionCount.text);
    objSelectionCount.text = (current + amount).ToString();
  }
}
