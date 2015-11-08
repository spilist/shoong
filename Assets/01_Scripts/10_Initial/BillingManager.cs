using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class BillingManager : MonoBehaviour {
  public static BillingManager bm;
  public CharactersMenu charactersMenu;

  private Dictionary<string, BillingProduct> bProducts;
  private   int         m_productIter;
  private   BillingProduct[]  m_products;
  private   bool        m_productRequestFinished;

  void Start () {
    bm = this;

    m_products          = NPSettings.Billing.Products;
    m_productRequestFinished  = false;

    bProducts = new Dictionary<string, BillingProduct>();

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
    if (_regProductsList != null)
    {
      m_productRequestFinished  = true;

      foreach (BillingProduct _eachProduct in _regProductsList) {
        bProducts[_eachProduct.ProductIdentifier] = _eachProduct;
      }
    }
  }

  public BillingProduct getProduct(string name) {
    return bProducts.ContainsKey(name) ? bProducts[name] : null;
  }

  private void DidReceiveTransactionInfoEvent (BillingTransaction[] _transactionList, string _error)
  {
    if (_transactionList != null)
    {
      eBillingTransactionState state;
      foreach (BillingTransaction _eachTransaction in _transactionList)
      {
        state = _eachTransaction.TransactionState;
        if (state == eBillingTransactionState.PURCHASED || state == eBillingTransactionState.RESTORED) {
          charactersMenu.buyComplete(_eachTransaction.ProductIdentifier, state == eBillingTransactionState.PURCHASED);
        }
      }
    }
  }
}
