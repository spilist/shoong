using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {
  public static AdsManager am;
  public int showAfter = 10;
  public int showPerMin = 2;
  private InterstitialAd interstitial;

  void Start () {
    am = this;
    if (available()) loadAds();
	}

  public void loadAds() {
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4666969549435607/8331172170";
    #elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        return;
    #else
        string adUnitId = "unexpected_platform";
    #endif

    interstitial = new InterstitialAd(adUnitId);

    // Register for ad events.
    // interstitial.AdLoaded += HandleInterstitialLoaded;

    // interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;

    // interstitial.AdOpened += HandleInterstitialOpened;
    // interstitial.AdClosing += HandleInterstitialClosing;

    // interstitial.AdClosed += HandleInterstitialClosed;

    // interstitial.AdLeftApplication += HandleInterstitialLeftApplication;

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder()
                          // .AddTestDevice(AdRequest.TestDeviceSimulator)
                          // .AddTestDevice("AAED8D9E90BDFE343EFB2A4EAD98E81E")
                          .Build();

    // Load the interstitial with the request.
    interstitial.LoadAd(request);
  }

  public void HandleInterstitialFailedToLoad(object sender, EventArgs args) {
    if (interstitial != null) interstitial.Destroy();
  }

  public void showGameOverAds() {
    if (available() && interstitial != null && interstitial.IsLoaded()) {
      interstitial.Show();
      DataManager.dm.setDateTime("LastDateTimeAdsSeen");
    }
  }

  bool available() {
    if (DataManager.dm.getInt("TotalNumPlays") < showAfter) return false;

    float minutesPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("LastDateTimeAdsSeen")).TotalMinutes;

    return minutesPassed >= showPerMin;
  }

  public void HandleInterstitialClosed(object sender, EventArgs args) {
    if (interstitial != null) interstitial.Destroy();
  }
}
