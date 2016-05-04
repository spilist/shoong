using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SocialPlatformManager : MonoBehaviour {
  public static SocialPlatformManager spm = null;
  public static SocialDataCache cache = null;
  public AchievementManager am; // If the user is not connected to GPGS or GameCenter, AchievementManager does nothing

  [System.Serializable]
  public class ProductInfo {
    public string GlobalId;
    public string AndroidId;
    public string AppleId;
  }
  public ProductInfo[] achievementInfos;
  public ProductInfo[] leaderboardInfos;
  public Dictionary<string, string> achievementInfoMap;
  public Dictionary<string, string> leaderboardInfoMap;

  void Awake() {
    if (spm != null && spm != this) {
      Destroy(gameObject);
      return;
    }
    
    DontDestroyOnLoad(gameObject);
    spm = this;
    cache = GetComponent<SocialDataCache>();

    achievementInfoMap = new Dictionary<string, string>();
    foreach (ProductInfo info in achievementInfos) {
#if UNITY_IOS
      achievementInfoMap.Add(info.GlobalId, info.AppleId);
#elif UNITY_ANDROID
      achievementInfoMap.Add(info.GlobalId, info.AndroidId);
#endif
    }
    leaderboardInfoMap = new Dictionary<string, string>();
    foreach (ProductInfo info in leaderboardInfos) {
#if UNITY_IOS
      leaderboardtInfoMap.Add(info.GlobalId, info.AppleId);
#elif UNITY_ANDROID
      leaderboardInfoMap.Add(info.GlobalId, info.AndroidId);
#endif
    }
    am = new AchievementManager();

    

#if UNITY_IOS
    // Initialize game center here
#elif UNITY_ANDROID
    PlayGamesPlatform.InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
    // recommended for debugging:
    PlayGamesPlatform.DebugLogEnabled = true;
    // Activate the Google Play Games platform
    PlayGamesPlatform.Activate();
#endif
  }

  public static bool isAuthenticated() {
#if UNITY_IOS
  // Write for Game Center
  return Social.localUser.authenticated == false;
#elif UNITY_ANDROID
  return PlayGamesPlatform.Instance.IsAuthenticated();
#endif
  }

  public void authenticate(System.Action<bool> onCompletion) {
    if (SocialPlatformManager.isAuthenticated() == false) {
      Social.localUser.Authenticate((bool _success)=>{
        if (_success) {
          // This line is required, or cannot load profile image. This is bug. https://github.com/playgameservices/play-games-plugin-for-unity/issues/1056
          string userName = Social.localUser.userName;
          Debug.Log("Sign-In Successfully");
          DataManager.dm.setBool("GoogleLoggedInSetting", false);
          //Social.LoadAchievements(ProcessLoadedAchievements);
          cache.init();
        } else {
          Debug.Log("Sign-In Failed");
          DataManager.dm.setBool("GoogleLoggedInSetting", true);
        }
        if (onCompletion != null)
          onCompletion(_success);
      });
    } else { // Already authenticated
      //onCompletion(true);
    }
  }

  public void signout() {
    if (SocialPlatformManager.isAuthenticated()) {
#if UNITY_IOS
     // Write for Game Center

#elif UNITY_ANDROID // Separated Android and IOS because Unity Social API does not have sign-out method
      PlayGamesPlatform.Instance.SignOut();
      Debug.Log("Local user is signed out successfully!");
#endif
    }
  }

  // This function gets called when the LoadAchievement call completes
  void ProcessLoadedAchievements(IAchievement[] achievements) {
    if (achievements.Length == 0)
      Debug.Log("Error: no achievements found");
    else
      Debug.Log("Got " + achievements.Length + " achievements");

    am.reportAllAchievements(achievements);
    
  }

  public void showAchievementUI() {
    Social.ShowAchievementsUI();
  }

  public void showLeaderboardUI() {
    Social.ShowLeaderboardUI();
  }
}
