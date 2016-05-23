using System;
using UnityEngine;
using System.Collections;
using Heyzap;

public class AdsManager : MonoBehaviour {
  public static AdsManager am;
  public int rewardAdShowAfter = 10;
  public int rewardShowPerMin = 2;
  public int gameOverAdShowAfter = 10;
  public int gameOverShowPerMin = 2;
  private bool gameOverPause = false;
  // private InterstitialAd interstitial;

  void Start () {
    am = this;
    // if (available()) loadAds2();
    loadAds2();
	}

  void loadAds2() {
    HeyzapAds.Start("d772c6e33d0e63212d4350fc7811d507", HeyzapAds.FLAG_NO_OPTIONS);
    // Debug.Log("HeyZap load try");
  }

  // public void loadAds() {
  //   // #if UNITY_ANDROID
  //       string adUnitId = "ca-app-pub-4666969549435607/8331172170";
  //   // #elif UNITY_IPHONE
  //       // string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
  //       // return;
  //   // #else
  //       // string adUnitId = "unexpected_platform";
  //   // #endif

  //   interstitial = new InterstitialAd(adUnitId);

  //   // Register for ad events.
  //   // interstitial.AdLoaded += HandleInterstitialLoaded;

  //   // interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;

  //   // interstitial.AdOpened += HandleInterstitialOpened;
  //   // interstitial.AdClosing += HandleInterstitialClosing;

  //   // interstitial.AdClosed += HandleInterstitialClosed;

  //   // interstitial.AdLeftApplication += HandleInterstitialLeftApplication;

  //   // Create an empty ad request.
  //   AdRequest request = new AdRequest.Builder()
  //                         // .AddTestDevice(AdRequest.TestDeviceSimulator)
  //                         // .AddTestDevice("AAED8D9E90BDFE343EFB2A4EAD98E81E")
  //                         .Build();

  //   // Load the interstitial with the request.
  //   interstitial.LoadAd(request);
  // }

  // public void HandleInterstitialFailedToLoad(object sender, EventArgs args) {
  //   if (interstitial != null) interstitial.Destroy();
  // }

  public IEnumerator showGameOverAds() {
    // if (available() && interstitial != null && interstitial.IsLoaded()) {
    // if (available()) {
    if (gameOverAvailable()) {
      HZShowOptions options = new HZShowOptions();
      HZIncentivizedAd.SetDisplayListener(gameOverAdsDisaplyListener);
      gameOverPause = true;
      HZInterstitialAd.Show();
      while (gameOverPause) {
        yield return new WaitForSeconds(0.1f);
      }

      DataManager.dm.setDateTime("GameOverLastDateTimeAdsSeen");
    }
    yield return null;
  }

  private void gameOverAdsDisaplyListener(string adState, string tag) {
    if (adState.Equals("show")) {
      // Sent when an ad has been displayed.
      // This is a good place to pause your app, if applicable.

    } else if (adState.Equals("hide")) {
      // Sent when an ad has been removed from view.
      // This is a good place to unpause your app, if applicable.
      gameOverPause = false;
    } else {
      // For various errors
      gameOverPause = false;
    }
  }

  void setGameOverPause(bool pause) {
    gameOverPause = pause;
  }

  public bool rewardAvailable() {
    if (DataManager.dm.getInt("TotalNumPlays") < rewardAdShowAfter) return false;

    float minutesPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("RewardLastDateTimeAdsSeen")).TotalMinutes;

    return minutesPassed >= rewardShowPerMin;
  }

  public void showedRewardAd() {
    DataManager.dm.setDateTime("RewardLastDateTimeAdsSeen");
  }

  public bool gameOverAvailable() {
    if (DataManager.dm.getInt("TotalNumPlays") < gameOverAdShowAfter) return false;

    float minutesPassed = (float) (DateTime.Now - DataManager.dm.getDateTime("GameOverLastDateTimeAdsSeen")).TotalMinutes;

    return minutesPassed >= gameOverShowPerMin;
  }

  // public void HandleInterstitialClosed(object sender, EventArgs args) {
  //   if (interstitial != null) interstitial.Destroy();
  // }
}
