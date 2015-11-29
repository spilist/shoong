using UnityEngine;
using System.Collections;

public class RestorePurchasesButton : MenusBehavior {
  override public void activateSelf() {
    BillingManager.bm.RestoreCompletedTransactions();
  }
}
