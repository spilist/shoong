using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ObjectBuyButton : MenusBehavior {
  public string which;
  public CubesYouHave cubes;
  public Color notAffordableTextColor;
  public Text priceText;
  public BuyButtonsCubeIconPosition icon;

  private UIObjects selectedObj;
  private int price;
  private string objectName;
  private bool affordable = true;

	public void setObject(UIObjects obj) {
    gameObject.SetActive(true);
    selectedObj = obj;
    objectName = obj.transform.parent.name;

    price = obj.getPrice(which);
    priceText.text = price.ToString("N0");
    icon.adjust(priceText);

    if (cubes.youHave() < price) {
      affordable = false;
      priceText.color = notAffordableTextColor;
      stopBlink();
    } else {
      affordable = true;
      priceText.color = new Color(255, 255, 255);
      if (which == "normal") {
        startBlink();
      }
    }
  }

  override public void activateSelf() {
    if (!affordable) return;

    string objLevel = objectName + "Level";
    if (DataManager.dm.getInt(objLevel) == 0) {
      DataManager.dm.setBool(objectName, true);
      string tutorialsNotDone = PlayerPrefs.GetString("ObjectTutorialsNotDone");
      PlayerPrefs.SetString("ObjectTutorialsNotDone", (tutorialsNotDone + " " + objectName).Trim());
    }
    DataManager.dm.increment(objLevel);

    DataManager.dm.setDateTime(objLevel + DataManager.dm.getInt(objLevel));

    cubes.buy(price);
    selectedObj.buy();

    DataManager.dm.save();
  }
}
