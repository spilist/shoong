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
  private Vector3 origInnerPos;

  override public void initializeRest() {
    origInnerPos = innerText.localPosition;
    checkAutoBought();

    if (DataManager.dm.getInt("TotalNumPlays") <= showHelpFor && !DataManager.dm.getBool(settingName + "Setting")) {
      autoBoosterHelp.SetActive(true);
    }

    priceCount.text = "-" + price;
  }

  override public void applyStatus() {
    if (clicked) {
      if (DataManager.dm.getBool("AutoBoosterPurchased") || GoldManager.gm.getCount() >= price) {
        filter.sharedMesh = activeMesh;
        available = true;
        autoBoosterHelp.SetActive(false);
        innerText.GetComponent<Text>().text = description() + " ON";

        if (!DataManager.dm.getBool("AutoBoosterPurchased")) showGold();
      } else {
        filter.sharedMesh = notEnoughMesh;
        available = false;
        innerText.GetComponent<Text>().text = description() + " ON";
        showGold();
      }
    } else {
      filter.sharedMesh = inactiveMesh;
      available = false;
      innerText.GetComponent<Text>().text = description() + " OFF";
      hideGold();
    }
  }

  string description() {
    return innerText.GetComponent<LocalText>().origString;
  }

  public bool isOn() {
    return available;
  }

  public bool decrementGold() {
    return available && !DataManager.dm.getBool("AutoBoosterPurchased");
  }

  public void checkAutoBought() {
    if (!DataManager.dm.getBool("AutoBoosterPurchased")) return;

    hideGold();
    GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y);
    purchaseAuto.SetActive(false);
    autoBoosterHelp.SetActive(false);
  }

  void hideGold() {
    innerText.localPosition = new Vector3(0, innerText.localPosition.y, innerText.localPosition.z);
    goldIcon.SetActive(false);
  }

  void showGold() {
    innerText.localPosition = origInnerPos;
    goldIcon.SetActive(true);
  }
}
