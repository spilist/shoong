using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;

public class IUserProfileTestImpl : IUserProfile {
  string idVal, userNameVal;
  bool isFriendVal;
  Texture2D imageVal;
  public IUserProfileTestImpl(string id, Texture2D image, bool isFriend, string userName) {
    idVal = id;
    imageVal = image;
    isFriendVal = isFriend;
    userNameVal = userName;
  }
  public string id {
    get {
      return idVal;
    }
  }

  public Texture2D image {
    get {
      return imageVal;
    }
  }

  public bool isFriend {
    get {
      return isFriendVal;
    }
  }

  public UserState state {
    get {
      return UserState.Online;
    }
  }

  public string userName {
    get {
      return userNameVal;
    }
  }
}
