using UnityEngine;
using System.Collections;

public class UIObjects : MenusBehavior {
  private ObjectsMenu objectsMenu;
  private string objectCategory;
  private ObjectsCategoryButton categoryButton;

  public string objectName;
  public string description;
  public string upgradeEffect;
  public int cubePrice = 10000;
  public int goldenCubePrice = 1000;

  public int[] prices;

  void Start() {
    objectsMenu = GameObject.Find("ObjectsMenu").GetComponent<ObjectsMenu>();
    objectCategory = transform.parent.parent.name;
  }

  override public void activateSelf() {
    objectsMenu.resetAll(objectCategory);
    GetComponent<Renderer>().enabled = true;
    objectsMenu.objDetail.selectObject(this);
    categoryButton = objectsMenu.transform.Find(objectCategory + "Button").GetComponent<ObjectsCategoryButton>();
  }

  public bool isActive() {
    return transform.parent.Find("ActiveBox").gameObject.activeSelf;
  }

  public void setActive(bool value) {
    transform.parent.Find("ActiveBox").gameObject.SetActive(value);
    objectsMenu.objDetail.checkBought(this);
    categoryButton.changeSelectionCount(value ? 1: -1);
    AudioSource.PlayClipAtPoint(objectsMenu.objectActiveSound, transform.position);
  }

  public void buy() {
    objectsMenu.objDetail.checkBought(this);
    AudioSource.PlayClipAtPoint(objectsMenu.objectBuySound, transform.position);
  }

  public int getPrice(string which) {
    int price = prices[DataManager.dm.getInt(transform.parent.name + "Level")];
    return (which == "golden") ? price / 100 : price;
  }
}
