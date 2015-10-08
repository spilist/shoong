using UnityEngine;
using System.Collections;

// Refered http://blogs.voxelbusters.com/products/cross-platform-native-plugins/game-services
using System.Collections.Generic;
using System;

public class NPBManager : MonoBehaviour {
  public AchievementManager am;
  bool isAuthenticating = false;

  public void init() {
    am = new AchievementManager();
  }
  
  public void authenticate(System.Action<bool> onCompletion) {
    // Authenticate to GPGS
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == false &&
        isAuthenticating == false) {
      isAuthenticating = true;
      NPBinding.GameServices.LocalUser.Authenticate((bool _success)=>{        
        if (_success) {
          Debug.Log("Sign-In Successfully");
          Debug.Log("Local User Details : " + NPBinding.GameServices.LocalUser.ToString());
        } else {
          Debug.Log("Sign-In Failed");
        }
        isAuthenticating = false;
        onCompletion(_success);
      });
    }
  }


}