using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class GPGSManager : MonoBehaviour {
  public AchievementManager am;
  bool isAuthenticating = false;
  
  
  public void init() {
    am = new AchievementManager();
    am.init();
    
    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
      // enables saving game progress.
      .EnableSavedGames()
        .Build();
    
    PlayGamesPlatform.InitializeInstance(config);
    // recommended for debugging:
    PlayGamesPlatform.DebugLogEnabled = true;
    // Activate the Google Play Games platform
    PlayGamesPlatform.Activate(); 
  }
  
  // Update is called once per frame
  void Update () {
    
  }
  
  public void authenticate(System.Action<bool> onCompletion) {
    // Authenticate to GPGS
    // authenticate user:
    
    Social.localUser.Authenticate((bool success) => {
      onCompletion(success);
    });
  }
}
