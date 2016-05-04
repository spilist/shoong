using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using System;

public class BillingManager : MonoBehaviour, IStoreListener {
  public static BillingManager bm;
  private static IStoreController m_StoreController;  // Reference to the Purchasing system.
  private static IExtensionProvider m_StoreExtensionProvider;
  public CharactersMenu charactersMenu;
  private bool isOnPurchasing = false; // This is for distinguising purchase restore

  [System.Serializable]
  public class ProductInfo {
    public string GlobalId;
    public ProductType type = ProductType.NonConsumable;
    public string AndroidId;
    public string AppleId;
  }
  public ProductInfo[] productInfos;
  private ProductDefinition[] m_products;
  private Dictionary<string, Product> m_products_map;

  void Awake() {
    if (bm != null && bm != this) {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    bm = this;
  }

  void Start() {
    m_products = new ProductDefinition[productInfos.Length];
    for (int i = 0; i < productInfos.Length; i++) {
#if UNITY_IOS
      m_products[i] = new ProductDefinition(productInfos[i].GlobalId, productInfos[i].AppleId, productInfos[i].type);
#elif UNITY_ANDROID
      m_products[i] = new ProductDefinition(productInfos[i].GlobalId, productInfos[i].AndroidId, productInfos[i].type);
#endif
    }
    RequestBillingProducts();
  }

  public void InitializePurchasing() {
    // If we have already connected to Purchasing ...
    if (IsInitialized()) {
      // ... we are done here.
      Debug.Log("InitializePurchasing finished");
      return;
    }

    // Create a builder, first passing in a suite of Unity provided stores.
    var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

    // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
    builder.AddProducts(m_products);
    UnityPurchasing.Initialize(this, builder);
  }

  private bool IsInitialized() {
    // Only say we are initialized if both the Purchasing references are set.
    return m_StoreController != null && m_StoreExtensionProvider != null;
  }

  private void RequestBillingProducts() {
    // If we haven't set up the Unity Purchasing reference
    if (m_StoreController == null) {
      // Begin to configure our connection to Purchasing
      InitializePurchasing();
    }
  }

  public void BuyProductID(string productId) {
    isOnPurchasing = true;
    // If the stores throw an unexpected exception, use try..catch to protect my logic here.
    try {
      // If Purchasing has been initialized ...
      if (IsInitialized()) {
        // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
        Product product = m_StoreController.products.WithID(productId);

        // If the look up found a product for this device's store and that product is ready to be sold ... 
        if (product != null && product.availableToPurchase) {
          Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
          m_StoreController.InitiatePurchase(product);
        }
        // Otherwise ...
        else {
          // ... report the product look-up failure situation  
          Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
        }
      }
      // Otherwise ...
      else {
        // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
        Debug.Log("BuyProductID FAIL. Not initialized.");
      }
    }
    // Complete the unexpected exception handling ...
    catch (Exception e) {
      // ... by reporting any unexpected exception for later diagnosis.
      Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
    }
  }

  // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
  public void RestorePurchases() {
    // If Purchasing has not yet been set up ...
    if (!IsInitialized()) {
      // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
      Debug.Log("RestorePurchases FAIL. Not initialized.");
      return;
    }

    // If we are running on an Apple device ... 
    if (Application.platform == RuntimePlatform.IPhonePlayer ||
        Application.platform == RuntimePlatform.OSXPlayer) {
      // ... begin restoring purchases
      Debug.Log("RestorePurchases started ...");

      // Fetch the Apple store-specific subsystem.
      var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
      // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
      apple.RestoreTransactions((result) => {
        // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
        Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
      });
    }
    // Otherwise ...
    else {
      // We are not running on an Apple device. No work is necessary to restore purchases.
      Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
    }
  }

  public bool IsProductPurchased(string _productIdentifier) {
    RequestBillingProducts();
    return m_StoreController.products.WithID(_productIdentifier).hasReceipt;
  }

  public Product getProduct(string name) {
    return m_products_map.ContainsKey(name) ? m_products_map[name] : null;
  }

  //  
  // --- IStoreListener
  //

  public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
    // Purchasing has succeeded initializing. Collect our Purchasing references.
    Debug.Log("OnInitialized: PASS");
    // Update m_products_map that would be used by UICharacters
    m_products_map = new Dictionary<string, Product>();
    foreach (Product product in controller.products.all)
      m_products_map.Add(product.definition.id, product);

    // Overall Purchasing system, configured with products for this application.
    m_StoreController = controller;
    // Store specific subsystem, for accessing device-specific store features.
    m_StoreExtensionProvider = extensions;
  }


  public void OnInitializeFailed(InitializationFailureReason error) {
    // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
    Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
  }


  public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
    //Debug.Log("ProcessPurchase: " + "transactionID(" + args.purchasedProduct.transactionID + "), productId(" + args.purchasedProduct.definition.id + ")");
    if (charactersMenu.buyComplete(args.purchasedProduct.transactionID, args.purchasedProduct.definition.id, isOnPurchasing)) {
      Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
    } else {
      Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
    }// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.

    if (isOnPurchasing) isOnPurchasing = false;
    return PurchaseProcessingResult.Complete;
  }


  public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
    // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
    Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    if (isOnPurchasing) isOnPurchasing = false;
  }
}
