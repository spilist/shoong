using UnityEngine;
using System.Collections;

public class UnlockAutoMode : MenusBehavior {
	public AutoBoosterButton abb;
  private string id = "unlock_automode";

  override public void activateSelf() {
    BillingManager.bm.BuyProduct(id);
    FacebookManager.fb.initiateCheckout(BillingManager.bm.getProduct(id), "Unlock Auto Booster");
  }

  public void buyComplete() {
    FacebookManager.fb.purchase(BillingManager.bm.getProduct(id), "Unlock Auto Booster");

    DataManager.dm.setBool("AutoBoosterPurchased", true);
    DataManager.dm.save();
    // abb.checkAutoBought();
  }
}
