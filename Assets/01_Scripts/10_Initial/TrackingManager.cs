#undef UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

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
    tm = this;

#if !UNITY_EDITOR
    if (!FB.IsInitialized) {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    } else {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }
    googleAnalyticsV3 = GetComponent<GoogleAnalyticsV3>();
    if (!googleAnalyticsV3.isActiveAndEnabled)
      initGoogleAnalytics();
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
    googleAnalyticsV3.logLevel = GoogleAnalyticsV3.DebugMode.VERBOSE;
    // dryRun is for test; If it is true, the logs are not sent to server
    googleAnalyticsV3.dryRun = false;
    // Need to be enabled here, to set the variables above then run the Start() of GoogleAnalyticsV3.
    // If the GoogleAnalyticsV3 is updated, we should change its Awake() to Start(), so it would run on enable.
    googleAnalyticsV3.enabled = true;
    googleAnalyticsV3.StartSession();    
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
    
    new TrackingFacade("Play Game", 1)
      .addEvent("PlayCharacter", CharacterManager.cm.getCurrentCharacter())
      .addEvent("isRandom", CharacterManager.cm.isRandom)
      .addEvent("isHard", CubeManager.cm.getBonus() > 0)
      .addEvent("isAuto", DataManager.dm.getBool("AutoBoosterSetting"))
      .addEvent("Skill", SkillManager.sm.skillName())
      .addEvent("Phase", PhaseManager.pm.phase() + 1)
      .addEvent("Score", CubeManager.cm.getCount() + CubeManager.cm.getBonus())
      .addEvent("Time", TimeManager.time.now)
      .addEvent("BoosterSuccessRate", ((float)(Player.pl.numBoosters)) / (Player.pl.numBoosters + RhythmManager.rm.failedBeatCount))
      .addEvent("Total Plays", DataManager.dm.getInt("TotalNumPlays"))
      .addEvent("Total PlayingTime", DataManager.dm.getInt("TotalTime"))
      .addEvent("Gold Earned", GoldManager.gm.earned())
      .logEvent();
    
#endif

    #if UNITY_EDITOR

    Debug.Log("Phase: " + (PhaseManager.pm.phase() + 1));
    Debug.Log("PlayCharacter: " + CharacterManager.cm.getCurrentCharacter());
    Debug.Log("isRandom: " + CharacterManager.cm.isRandom);
    Debug.Log("isHard: " + (CubeManager.cm.getBonus() > 0));
    Debug.Log("isAuto: " + DataManager.dm.getBool("AutoBoosterSetting"));
    Debug.Log("Skill: " + SkillManager.sm.skillName());
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
    
    new TrackingFacade("Create Toy", 1).addEvent("Total Creations", numCreate)
      .addEvent(AppEventParameterName.ContentID, name)
      .addEvent(AppEventParameterName.ContentType, rarity)
      .addEvent("Is a new toy?", isNewToy)
      .logEvent();
    #endif
  }

  public void firstPlayLog(string description) {
#if !UNITY_EDITOR

    new TrackingFacade("FirstPlay", description, "", 0)
      .logEvent();
    
    #endif
  }

  public void tutorialDone(bool skipped) {
#if !UNITY_EDITOR

    new TrackingFacade(AppEventName.CompletedTutorial, 0)
      .addEvent("Skipped", skipped)
      .logEvent();

    #endif
  }

  public void initiateCheckout(BillingProduct bProduct, string rarity) {
#if !UNITY_EDITOR

    new TrackingFacade(AppEventName.InitiatedCheckout, bProduct.Price)
      .addEvent(AppEventParameterName.ContentID, bProduct.ProductIdentifier)
      .addEvent(AppEventParameterName.Currency, bProduct.CurrencyCode)
      .addEvent(AppEventParameterName.ContentType, rarity)
      .logEvent();

    #endif
  }

  public void purchase(string transactionId, BillingProduct bProduct, string rarity) {
#if !UNITY_EDITOR

    googleAnalyticsV3.LogItem(new ItemHitBuilder()
      .SetTransactionID(transactionId)
      .SetName(bProduct.Name)
      .SetSKU(bProduct.ProductIdentifier)
      .SetCategory("Toy_" + rarity)
      .SetPrice(bProduct.Price)
      .SetQuantity(1)
      .SetCurrencyCode(bProduct.CurrencyCode));
      
    FB.LogPurchase(
      bProduct.Price,
      bProduct.CurrencyCode,
      new Dictionary<string, object>() {
        { "TransactionId", transactionId },
        { AppEventParameterName.ContentID, bProduct.ProductIdentifier },
        { AppEventParameterName.ContentType, rarity }
      });
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
      Dictionary<string, object> FBParams = new Dictionary<string, object>();
      foreach(EventHitBuilder builder in events)
      {
        if (builder.GetEventLabel() == "")
          FBParams.Add(builder.GetEventAction(), builder.GetEventValue());
        else
          FBParams.Add(builder.GetEventAction(), builder.GetEventLabel());
        googleAnalyticsV3.LogEvent(builder);
      }
      FB.LogAppEvent(eventCategory, events[0].GetEventValue(), FBParams);
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
