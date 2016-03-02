using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoBoosterButton : OnOffButton {
  public GameObject autoBoosterHelp;
  public int showHelpFor = 4;
  private bool available;
  private string id = "unlock_automode";

  override public void initializeRest() {
    // if (DataManager.dm.getInt("TotalNumPlays") <= showHelpFor && !DataManager.dm.getBool(settingName + "Setting")) {
    //   autoBoosterHelp.SetActive(true);
    // }
  }

  override public void activateSelf() {
    if (DataManager.dm.getBool("AutoBoosterPurchased")) {
      clicked = !clicked;
      DataManager.dm.setBool(settingName + "Setting", clicked);
      applyStatus();
      DataManager.dm.save();
    } else {
      BillingManager.bm.BuyProduct(id);
      TrackingManager.tm.initiateCheckout(BillingManager.bm.getProduct(id), "Unlock Auto Booster");
    }
  }

  override public void applyStatus() {
    if (clicked) {
      filter.sharedMesh = activeMesh;
      available = true;
      // autoBoosterHelp.SetActive(false);
    } else {
      filter.sharedMesh = inactiveMesh;
      available = false;
    }
  }

  public bool isOn() {
    return available;
  }

  public void buyComplete(string transactionId, bool bought) {
    if (bought) TrackingManager.tm.purchase(transactionId, BillingManager.bm.getProduct(id), "Unlock Auto Booster");
    DataManager.dm.setBool("AutoBoosterPurchased", true);
    DataManager.dm.save();
  }
}
