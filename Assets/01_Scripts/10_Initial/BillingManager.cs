using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class BillingManager : MonoBehaviour {
  public static BillingManager bm;

  private   int         m_productIter;
  private   BillingProduct[]  m_products;
  private   bool        m_productRequestFinished;

  void Start () {
    bm = this;

    m_products          = NPSettings.Billing.Products;
    m_productRequestFinished  = false;

    RequestBillingProducts(m_products);
	}

  void OnEnable() {
    Billing.DidFinishProductsRequestEvent += DidFinishProductsRequestEvent;
    Billing.DidReceiveTransactionInfoEvent  += DidReceiveTransactionInfoEvent;
  }

  void OnDisable() {
    Billing.DidFinishProductsRequestEvent -= DidFinishProductsRequestEvent;
    Billing.DidReceiveTransactionInfoEvent  -= DidReceiveTransactionInfoEvent;
  }

  private void RequestBillingProducts (BillingProduct[]  _products)
  {
    NPBinding.Billing.RequestForBillingProducts (_products);
  }

  public void RestoreCompletedTransactions ()
  {
    NPBinding.Billing.RestoreCompletedTransactions ();
  }

  public void BuyProduct (string _productIdentifier)
  {
    if (m_productRequestFinished) {
      NPBinding.Billing.BuyProduct (_productIdentifier);
    }
  }

  public bool IsProductPurchased (string _productIdentifier)
  {
    return NPBinding.Billing.IsProductPurchased (_productIdentifier);
  }

  private void DidFinishProductsRequestEvent (BillingProduct[] _regProductsList, string _error)
  {
    // AddNewResult(string.Format("Billing products request finished. Error = {0}.", _error.GetPrintableString()));

    if (_regProductsList != null)
    {
      m_productRequestFinished  = true;
    //   AppendResult (string.Format ("Totally {0} billing products information were received.", _regProductsList.Length));

    //   foreach (BillingProduct _eachProduct in _regProductsList)
    //     AppendResult (_eachProduct.ToString());
    }
  }

  private void DidReceiveTransactionInfoEvent (BillingTransaction[] _transactionList, string _error)
  {
    // AddNewResult(string.Format("Billing transaction finished. Error = {0}.", _error.GetPrintableString()));

    // if (_transactionList != null)
    // {
    //   AppendResult (string.Format ("Count of transaction information received = {0}.", _transactionList.Length));

    //   foreach (BillingTransaction _eachTransaction in _transactionList)
    //   {
    //     AppendResult ("Product Identifier = "     + _eachTransaction.ProductIdentifier);
    //     AppendResult ("Transaction State = "    + _eachTransaction.TransactionState);
    //     AppendResult ("Verification State = "   + _eachTransaction.VerificationState);
    //     AppendResult ("Transaction Date[UTC] = "  + _eachTransaction.TransactionDateUTC);
    //     AppendResult ("Transaction Date[Local] = "  + _eachTransaction.TransactionDateLocal);
    //     AppendResult ("Transaction Identifier = " + _eachTransaction.TransactionIdentifier);
    //     AppendResult ("Transaction Receipt = "    + _eachTransaction.TransactionReceipt);
    //     AppendResult ("Error = "          + _eachTransaction.Error.GetPrintableString());
    //   }
    // }
  }

  private BillingProduct GetCurrentProduct ()
  {
    return m_products[m_productIter];
  }

  private void GotoNextProduct ()
  {
    m_productIter++;

    if (m_productIter >= m_products.Length)
      m_productIter = 0;
  }

  private void GotoPreviousProduct ()
  {
    m_productIter--;

    if (m_productIter < 0)
      m_productIter = m_products.Length - 1;
  }

  private int GetProductsCount ()
  {
    return m_products.Length;
  }
}
