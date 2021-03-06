﻿using UnityEngine;
using System.Collections;

public class GoogleAuthManager : MonoBehaviour {
  private GoogleAuthManager manager;

  void Start () {
    if (manager != null && manager != this) {
      Destroy(gameObject);
      return;
    }
    manager = this;
    DontDestroyOnLoad(gameObject);
    // By the implementation of OnOffButton, 'false' actually means 'is logged in'
    /*
    if (DataManager.dm.getBool("GoogleLoggedInSetting") == false) {
      SocialPlatformManager.spm.authenticate((bool x)=> { });
    }
    */
    Debug.Log("Require sign in, " + SocialPlatformManager.isAuthenticated());
    SocialPlatformManager.spm.authenticate((bool x) => { });
  }
}
