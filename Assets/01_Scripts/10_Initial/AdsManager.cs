using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {
  public static AdsManager am;
  private InterstitialAd interstitial;

  void Start () {
    am = this;
    loadAds();
	}

  public void loadAds() {
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4666969549435607/8331172170";
    #elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
    #else
        string adUnitId = "unexpected_platform";
    #endif

    interstitial = new InterstitialAd(adUnitId);

    // Register for ad events.
    // interstitial.AdLoaded += HandleInterstitialLoaded;
    interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
    // interstitial.AdOpened += HandleInterstitialOpened;
    // interstitial.AdClosing += HandleInterstitialClosing;
    interstitial.AdClosed += HandleInterstitialClosed;
    // interstitial.AdLeftApplication += HandleInterstitialLeftApplication;

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder()
                          .AddTestDevice(AdRequest.TestDeviceSimulator)
                          .AddTestDevice("AAED8D9E90BDFE343EFB2A4EAD98E81E")
                          .Build();

    // Load the interstitial with the request.
    interstitial.LoadAd(request);
  }

  public void HandleInterstitialFailedToLoad(object sender, EventArgs args) {
    if (interstitial != null) interstitial.Destroy();
  }

  public void showGameOverAds() {
    if (interstitial != null && interstitial.IsLoaded()) {
      interstitial.Show();
    }
  }

  public void HandleInterstitialClosed(object sender, EventArgs args) {
    if (interstitial != null) interstitial.Destroy();
  }
}
