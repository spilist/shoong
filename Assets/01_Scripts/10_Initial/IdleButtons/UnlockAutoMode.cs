using UnityEngine;
using System.Collections;

public class UnlockAutoMode : MenusBehavior {
	public AutoBoosterButton abb;
  private string id = "unlock_automode";

  override public void activateSelf() {
    /*
    BillingManager.bm.BuyProduct(id);
    TrackingManager.tm.initiateCheckout(BillingManager.bm.getProduct(id), "Unlock Auto Booster");
    */
  }

  public void buyComplete(string transactionId, bool bought) {
    if (bought) TrackingManager.tm.purchase(transactionId, BillingManager.bm.getProduct(id), "Unlock Auto Booster");

    DataManager.dm.setBool("AutoBoosterPurchased", true);
    DataManager.dm.save();
    // abb.checkAutoBought();
  }
}
