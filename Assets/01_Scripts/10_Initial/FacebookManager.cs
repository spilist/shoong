using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class FacebookManager : MonoBehaviour {
  public static FacebookManager fb;

  void Awake() {
    if (fb != null && fb != this) {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    fb = this;

    if (!FB.IsInitialized) {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    } else {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }
  }

  private void InitCallback () {
      if (FB.IsInitialized) {
          // Signal an app activation App Event
          FB.ActivateApp();
          // Continue with Facebook SDK
          // ...
      } else {
          Debug.Log("Failed to Initialize the Facebook SDK");
      }
  }

  private void OnHideUnity (bool isGameShown) {
      // if (!isGameShown) {
      //     // Pause the game - we will need to hide
      //     Time.timeScale = 0;
      // } else {
      //     // Resume the game - we're getting focus again
      //     Time.timeScale = 1;
      // }
  }

  public void purchase(BillingProduct bProduct, string rarity) {
    FB.LogAppEvent(
        AppEventName.Purchased,
        bProduct.Price,
        new Dictionary<string, object>()
        {
          { AppEventParameterName.ContentID, bProduct.ProductIdentifier },
          { AppEventParameterName.Currency, bProduct.CurrencyCode },
          { AppEventParameterName.ContentType, rarity },
        });
  }
}
