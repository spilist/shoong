//#undef UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;

public class TrackingManager : MonoBehaviour {
  public static TrackingManager tm;
  private static GoogleAnalyticsV3 googleAnalyticsV3;
  private const string GA_PROPERTY_ID = "UA-71807640-3";

  void Awake() {
    if (tm != null && tm != this) {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    DontDestroyOnLoad(GetComponentInChildren<AppsFlyerTrackerCallbacks>());
    tm = this;

#if !UNITY_EDITOR
    googleAnalyticsV3 = GetComponent<GoogleAnalyticsV3>();
    if (!googleAnalyticsV3.isActiveAndEnabled)
      initGoogleAnalytics();

    initAppsFlyer();

#endif
  }

  private void initGoogleAnalytics()
  {
    // Application.bundleIdentifier may not work if we build it as standalone (pc) version
    // https://github.com/googleanalytics/google-analytics-plugin-for-unity/issues/97
    googleAnalyticsV3.androidTrackingCode = GA_PROPERTY_ID;
    googleAnalyticsV3.IOSTrackingCode = GA_PROPERTY_ID;
    googleAnalyticsV3.otherTrackingCode = GA_PROPERTY_ID;
    googleAnalyticsV3.productName = Application.productName;
    googleAnalyticsV3.bundleIdentifier = Application.bundleIdentifier;
    googleAnalyticsV3.bundleVersion = Application.version;
    googleAnalyticsV3.sendLaunchEvent = true;
    googleAnalyticsV3.dispatchPeriod = 5;
    //googleAnalyticsV3.logLevel = GoogleAnalyticsV3.DebugMode.VERBOSE;
    // dryRun is for test; If it is true, the logs are not sent to server
    googleAnalyticsV3.dryRun = false;
    // Need to be enabled here, to set the variables above then run the Start() of GoogleAnalyticsV3.
    // If the GoogleAnalyticsV3 is updated, we should change its Awake() to Start(), so it would run on enable.
    googleAnalyticsV3.enabled = true;
    googleAnalyticsV3.StartSession();
  }

  private void initAppsFlyer()
  {
#if UNITY_IOS

    AppsFlyer.setAppsFlyerKey ("PTuYBhA2CFm48vxR6SGRf7");
    AppsFlyer.setAppID ("YOUR_APPLE_APP_ID_HERE");
    AppsFlyer.getConversionData ();
    AppsFlyer.trackAppLaunch ();

#elif UNITY_ANDROID

    // if you are wotking without the manfest, you can initialize the SDK programattically.
    //AppsFlyer.init ("PTuYBhA2CFm48vxR6SGRf7");
    // All Initialization occur in the override activity defined in the mainfest.xml, including track app launch
    // You can define AppsFlyer library here use this commented out code.

    // un-comment this in case you are not working with the android manifest file
    //AppsFlyer.setAppID (Application.bundleIdentifier);

    // for getting the conversion data
    AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks","didReceiveConversionData", "didReceiveConversionDataWithError");

    // for in app billing validation
    AppsFlyer.createValidateInAppListener ("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure");

#endif

    print ("AppsFlyerId = " + AppsFlyer.getAppsFlyerId());
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

    new TrackingFacade("Play Game", 1)
      .addEvent("PlayCharacter", CharacterManager.cm.getCurrentCharacter())
      .addEvent("isRandom", CharacterManager.cm.isRandom)
      .addEvent("isHard", CubeManager.cm.getBonus() > 0)
      .addEvent("Skill", SkillManager.sm.skillName())
      .addEvent("Phase", PhaseManager.pm.phase() + 1)
      .addEvent("Score", CubeManager.cm.getCount() + CubeManager.cm.getBonus())
      .addEvent("Time", TimeManager.time.now)
      .addEvent("Total Plays", DataManager.dm.getInt("TotalNumPlays"))
      .addEvent("Total PlayingTime", DataManager.dm.getInt("TotalTime"))
      .addEvent("Gold Earned", GoldManager.gm.earned())
      .addEvent("GameOverReason", ScoreManager.sm.lastGameOverReason)
      .logEvent();

#endif
  }

  public void createToy(int numCreate, string rarity, string name, bool isNewToy) {
#if !UNITY_EDITOR

    new TrackingFacade("Create Toy", 1).addEvent("Total Creations", numCreate)
      .addEvent("Content ID", name)
      .addEvent("Content Type", rarity)
      .addEvent("Is a new toy?", isNewToy)
      .logEvent();
    #endif
  }

  public void firstPlayLog(string description) {
#if !UNITY_EDITOR

    new TrackingFacade("FirstPlay", description, "", 0)
      .logEvent();

    #else
    Debug.Log(description);
    #endif
  }

  public void tutorialDone(bool skipped) {
#if !UNITY_EDITOR

    new TrackingFacade("CompletedTutorial", 0)
      .addEvent("Skipped", skipped)
      .logEvent();

    #endif
  }

  public void initiateCheckout(Product bProduct, string rarity) {
#if !UNITY_EDITOR

    new TrackingFacade("InitiatedCheckout", (long) bProduct.metadata.localizedPrice)
      .addEvent("Content ID", bProduct.definition.id)
      .addEvent("Currency", bProduct.metadata.isoCurrencyCode)
      .addEvent("Content Type", rarity)
      .logEvent();

    #endif
  }

  public void purchase(string transactionId, Product bProduct, string rarity) {
#if !UNITY_EDITOR

    googleAnalyticsV3.LogItem(new ItemHitBuilder()
      .SetTransactionID(transactionId)
      .SetName(bProduct.definition.id)
      .SetSKU(bProduct.definition.id)
      .SetCategory("Toy_" + rarity)
      .SetPrice((long) bProduct.metadata.localizedPrice)
      .SetQuantity(1)
      .SetCurrencyCode(bProduct.metadata.isoCurrencyCode));

    /* Purchase tracking would be done in Google and Facebook
    Dictionary<string, string> purchaseEvent = new System.Collections.Generic.Dictionary<string, string>();
    purchaseEvent.Add("af_currency", bProduct.metadata.isoCurrencyCode);
    purchaseEvent.Add("af_revenue", bProduct.metadata.localizedPrice + "");
    purchaseEvent.Add("af_quantity", "1");
    AppsFlyer.trackRichEvent("af_purchase", purchaseEvent);
    */

#endif
  }

  public void rewardAdButtonShowed() {
#if !UNITY_EDITOR
    new TrackingFacade("RewardAd", 1)
      .addEvent("RewardAdShowed", 1)
      .logEvent();
#endif
  }

  public void rewardAdShowed() {
#if !UNITY_EDITOR
    new TrackingFacade("RewardAd", 1)
      .addEvent("RewardAdClicked", 1)
      .logEvent();
#endif
  }

  // Because of the difference of Facebook AppEvent and Google Analytics,
  // the eventValue is reported in different manner.
  // 1. Facebook AppEvent: Value parameter is reported of that used for instantiation
  // 2. Google Analytics: Values of each action is reported separately
  class TrackingFacade
  {
    public enum Types { Event }; //, Purchase };
    Types type;
    string eventCategory;
    List<EventHitBuilder> events;

    public TrackingFacade(string eventCategory, string eventAction, string eventLabel, long eventValue)
    {
      this.type = Types.Event;
      this.eventCategory = eventCategory;
      events = new List<EventHitBuilder>();
      addEvent(eventAction, eventLabel, eventValue);
    }

    // Set the first log for Google Analytics to have same category, action and label names
    public TrackingFacade(string eventCategory, long eventValue)
    {
      this.type = Types.Event;
      this.eventCategory = eventCategory;
      events = new List<EventHitBuilder>();
      addEvent(eventCategory, eventCategory, eventValue);
    }

    public TrackingFacade(string eventCategory, float eventValue)
    {
      this.type = Types.Event;
      this.eventCategory = eventCategory;
      events = new List<EventHitBuilder>();
      addEvent(eventCategory, eventCategory, (long) (eventValue * 100));
    }

    public TrackingFacade addEvent(string action, string label, long value)
    {
      EventHitBuilder builder = new EventHitBuilder();
      events.Add(new EventHitBuilder()
        .SetEventCategory(eventCategory)
        .SetEventAction(action)
        .SetEventLabel(label)
        .SetEventValue(value));
      return this;
    }
    public TrackingFacade addEvent(string action, string label)
    {
      EventHitBuilder builder = new EventHitBuilder();
      events.Add(new EventHitBuilder()
        .SetEventCategory(eventCategory)
        .SetEventAction(action)
        .SetEventLabel(label));
      return this;
    }
    public TrackingFacade addEvent(string action, long value)
    {
      EventHitBuilder builder = new EventHitBuilder();
      events.Add(new EventHitBuilder()
        .SetEventCategory(eventCategory)
        .SetEventAction(action)
        .SetEventValue(value));
      return this;
    }
    // If the value is boolean, convert it to 0 or 1
    public TrackingFacade addEvent(string action, bool value)
    {
      return addEvent(action, System.Convert.ToInt32(value));
    }

    public TrackingFacade addEvent(string action, float value)
    {
      return addEvent(action, (long)(value * 100));
    }

    public bool logEvent()
    {
      if (events.Count == 0)
      {
        Debug.LogError("TrackingFacade: There is no event to log");
        return false;
      }

      foreach(EventHitBuilder builder in events) {
        if (builder.Validate() == null) {
          Debug.LogError("TrackingFacade: Category or action is not set in the event. The entire event is ignored");
          return false;
        }
      }
      foreach(EventHitBuilder builder in events)
      {
        googleAnalyticsV3.LogEvent(builder);
      }
      sendEvent();
      return true;
    }

    // This may not be required, because it is periodically sended on iOS and Android
    public void sendEvent()
    {
      googleAnalyticsV3.DispatchHits();
    }
  }
}
