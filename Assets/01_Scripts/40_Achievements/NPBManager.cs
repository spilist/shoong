using UnityEngine;
using System.Collections;

// Refered http://blogs.voxelbusters.com/products/cross-platform-native-plugins/game-services
using System.Collections.Generic;
using System;
using VoxelBusters.Utility;

// Maybe we can use this interface to connect Facebook or Game Center
using VoxelBusters.NativePlugins;


public class NPBManager : MonoBehaviour {
  public AchievementManager am;
  public void init() {
    am = new AchievementManager();
    am.init();
  }

  public bool isAuthenticated() {
    return NPBinding.GameServices.LocalUser.IsAuthenticated;
  }
  
  public void authenticate(System.Action<bool, string> onCompletion) {
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == false) {
      NPBinding.GameServices.LocalUser.Authenticate((bool _success, string _error)=>{        
        if (_success) {
          Debug.Log("Sign-In Successfully");
          Debug.Log("Local User Details : " + NPBinding.GameServices.LocalUser.ToString());
          DataManager.dm.setBool("GoogleLoggedInSetting", false);
        } else {
          Debug.Log("Sign-In Failed: " + _error.GetPrintableString());
          DataManager.dm.setBool("GoogleLoggedInSetting", true);
        }
        if (onCompletion != null)
          onCompletion(_success, _error);
      });
    } else { // Already authenticated
      onCompletion(true, null);
    }
  }

  public void signout(System.Action<bool, string> onCompletion) {
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == true) {      
      NPBinding.GameServices.LocalUser.SignOut((bool _success, string _error)=>{        
        if (_success)
        {
          Debug.Log("Local user is signed out successfully!");
        }
        else
        {
          Debug.Log("Request to signout local user failed.");
          Debug.Log(string.Format("Error= {0}.", _error.GetPrintableString()));
        }
        if (onCompletion != null)
          onCompletion(_success, _error);
      });
    }
  }

  public void showAchievementUI(System.Action<string> onCompletion) {
    NPBinding.GameServices.ShowAchievementsUI((string _error)=>{
      if(_error != null) {
        Debug.Log("Closed achievements UI.");
        if (onCompletion != null)
          onCompletion(null);
      }
      else {
        Debug.Log("Error = " + _error);
        if (onCompletion != null)
          onCompletion(_error);
      }
    });
  }
  
  public void showRankingUI(System.Action<string> onCompletion) {
    NPBinding.GameServices.ShowLeaderboardUIWithGlobalID(AchievementManager.LB_SINGLE,
                                                         eLeaderboardTimeScope.ALL_TIME,
                                                         (string _error)=>{
      if(_error != null) {
        Debug.Log("Closed leaderboard UI.");
        if (onCompletion != null)
          onCompletion(null);
      }
      else {
        Debug.Log("Error = " + _error);
        if (onCompletion != null)
          onCompletion(_error);
      }
    });
  }
}