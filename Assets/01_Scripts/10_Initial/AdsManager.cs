using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {
  public static AdsManager am;

  public string adsID = "ca-app-pub-4666969549435607/8331172170";
  private InterstitialAd interstitial;

  void Start () {
    am = this;
    loadAds();
	}

  public void loadAds() {
    // Initialize an InterstitialAd.
    interstitial = new InterstitialAd(adsID);

    // Register for ad events.
    // interstitial.AdLoaded += HandleInterstitialLoaded;
    // interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
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

  public void showGameOverAds() {
    while(!interstitial.IsLoaded()) {}
    interstitial.Show();
  }

  public void HandleInterstitialClosed(object sender, EventArgs args) {
    interstitial.Destroy();
  }
}
