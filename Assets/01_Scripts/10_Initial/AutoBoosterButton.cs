using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoBoosterButton : OnOffButton {
  public Transform innerText;
  public GameObject goldIcon;
  public GameObject autoBoosterHelp;
  public GameObject purchaseAuto;
  public int showHelpFor = 4;
  public Mesh notEnoughMesh;
  public int price = 20;
  public Text priceCount;
  private bool available;

  override public void initializeRest() {
    checkAutoBought();

    // if (true) {
    if (DataManager.dm.getInt("TotalNumPlays") <= showHelpFor && !DataManager.dm.getBool(settingName + "Setting")) {
      autoBoosterHelp.SetActive(true);
    }

    priceCount.text = "-" + price;
  }

  override public void applyStatus() {
    if (clicked) {
      if (DataManager.dm.getBool("AutoBoosterPurchased") || GoldManager.gm.getCount() > price) {
        filter.sharedMesh = activeMesh;
        available = true;
        autoBoosterHelp.SetActive(false);
      } else {
        filter.sharedMesh = notEnoughMesh;
        available = false;
      }
    } else {
      filter.sharedMesh = inactiveMesh;
      available = false;
    }
  }

  public bool isOn() {
    return available;
  }

  public bool decrementGold() {
    // return true;
    return available && !DataManager.dm.getBool("AutoBoosterPurchased");
  }

  public void checkAutoBought() {
    if (!DataManager.dm.getBool("AutoBoosterPurchased")) return;

    innerText.localPosition = new Vector3(innerText.localPosition.x, innerText.localPosition.y, 0);
    goldIcon.SetActive(false);
    GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y);
    purchaseAuto.SetActive(false);
    autoBoosterHelp.SetActive(false);
  }
}
