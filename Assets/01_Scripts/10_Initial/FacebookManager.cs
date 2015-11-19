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

    #if !UNITY_EDITOR
    if (!FB.IsInitialized) {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    } else {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }

    #endif
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
    if (!isGameShown) {
        // Pause the game - we will need to hide
        Time.timeScale = 0;
    } else {
        // Resume the game - we're getting focus again
        Time.timeScale = 1;
    }
  }

  public void gameDone() {
    #if !UNITY_EDITOR

    FB.LogAppEvent(
      "Play Game",
      1,
      new Dictionary<string, object>() {
        { "Phase", PhaseManager.pm.phase() + 1},
        { "AutoRhythm", CubeManager.cm.getBonus() > 0},
        { "Score", CubeManager.cm.getCount() + CubeManager.cm.getBonus()},
        { "Time", TimeManager.time.now},
        { "BoosterSuccessRate", ((float)(Player.pl.numBoosters)) / (Player.pl.numBoosters + RhythmManager.rm.failedBeatCount) },
        { "Total Plays", DataManager.dm.getInt("TotalNumPlays")},
        { "Total PlayingTime", DataManager.dm.getInt("TotalTime")},
        { "Gold Earned", GoldManager.gm.earned()}
      });
    #endif

    #if UNITY_EDITOR

    Debug.Log("Phase: " + (PhaseManager.pm.phase() + 1));
    Debug.Log("Score: " + CubeManager.cm.getCount());
    Debug.Log("Time: " + TimeManager.time.now);
    Debug.Log("BoosterSuccessRate: " + 100 * ((float)(Player.pl.numBoosters)) / (Player.pl.numBoosters + RhythmManager.rm.failedBeatCount));
    Debug.Log("Total Plays: " + DataManager.dm.getInt("TotalNumPlays"));
    Debug.Log("Total PlayingTime: " + DataManager.dm.getInt("TotalTime"));
    Debug.Log("Gold Earned: " + GoldManager.gm.earned());

    #endif
  }

  public void createToy(int numCreate, string rarity, string name, bool isNewToy) {
    #if !UNITY_EDITOR

    FB.LogAppEvent(
      "Create Toy",
      1,
      new Dictionary<string, object>() {
        { "Total Creations", numCreate },
        { AppEventParameterName.ContentID, name },
        { AppEventParameterName.ContentType, rarity },
        { "Is a new toy?", isNewToy },
      });
    #endif
  }

  public void firstPlayLog(string description) {
    #if !UNITY_EDITOR

    FB.LogAppEvent(
      "FirstPlay_" + description,
      null,
      null);
    #endif
  }

  public void tutorialDone(bool skipped) {
    #if !UNITY_EDITOR

    FB.LogAppEvent(
      AppEventName.CompletedTutorial,
      null,
      new Dictionary<string, object>() {
        { "Skipped", skipped },
      });

    #endif
  }

  public void initiateCheckout(BillingProduct bProduct, string rarity) {
    #if !UNITY_EDITOR

    FB.LogAppEvent(
      AppEventName.InitiatedCheckout,
      bProduct.Price,
      new Dictionary<string, object>() {
        { AppEventParameterName.ContentID, bProduct.ProductIdentifier },
        { AppEventParameterName.Currency, bProduct.CurrencyCode },
        { AppEventParameterName.ContentType, rarity },
      });
    #endif
  }

  public void purchase(BillingProduct bProduct, string rarity) {
    #if !UNITY_EDITOR
    FB.LogPurchase(
      bProduct.Price,
      bProduct.CurrencyCode,
      new Dictionary<string, object>() {
        { AppEventParameterName.ContentID, bProduct.ProductIdentifier },
        { AppEventParameterName.ContentType, rarity }
      });
    #endif
  }
}
