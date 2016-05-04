using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System.Linq;
using System;
using System.Threading;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class SocialDataCache : MonoBehaviour {
  private DateTime lastCachedTime;
  public IUserProfile myProfile;
  public int MaxLoadCount;
  public UserScope userScope;
  private int loadedCount;
  int approxCount;
  bool friendDataLoaded = false;
  ILeaderboard lb;
  public Dictionary<string, IUserProfile> userIdToProfileCache = new Dictionary<string, IUserProfile>();
  public Dictionary<string, Sprite> userIdToAvatarCache = new Dictionary<string, Sprite>();
  Queue<IUserProfile> avartarLoadQueue = new Queue<IUserProfile>();
  
  void Start() {
    //StartCoroutine(loadAvatarsCoroutine());
  }

  void Update() {
  }

  public void init() {
    if (SocialPlatformManager.isAuthenticated()) {
      Debug.Log("SocialDataCache: Start initializing...");
#if UNITY_IOS
  // Write for Game Center
      lb = Social.CreateLeaderboard();
#elif UNITY_ANDROID
      lb = PlayGamesPlatform.Instance.CreateLeaderboard();
#endif
      lb.id = SocialPlatformManager.spm.leaderboardInfoMap[AchievementManager.LB_SINGLE];
      lb.range = new Range(1, MaxLoadCount);
      lb.userScope = userScope;
      lb.timeScope = TimeScope.AllTime;
      lb.LoadScores(loadFriendScores);
    } else {
      Debug.Log("SocialDataCache: Not authorized yet");
    }
  }

  // For using Social.Leaderboard
  void loadFriendScores(bool success) {
    IScore test = lb.localUserScore;
    string[] userIDs = lb.scores.Where(x => !userIdToProfileCache.ContainsKey(x.userID))
                             .Select(x => x.userID)
                             .Concat(new[] { Social.localUser.id }).ToArray();
    Debug.Log("Scores loaded count: " + lb.scores.Length);
    Debug.Log("Asking friends count: " + userIDs.Length);
    Social.LoadUsers(userIDs, loadFriendInfos);
  }

  void loadFriendInfos(IUserProfile[] userProfiles) {
    foreach (IUserProfile profile in userProfiles) {
      if (!userIdToProfileCache.ContainsKey(profile.id))
        userIdToProfileCache.Add(profile.id, profile);
      if (!userIdToAvatarCache.ContainsKey(profile.id)) {
        if (profile.image == null) {
          avartarLoadQueue.Enqueue(profile);
        }
      }
    }
    myProfile = userIdToProfileCache[Social.localUser.id];
    Debug.Log("Friends loaded count: " + userProfiles.Length);
    friendDataLoaded = true;
  }

  // Load one by one
  IEnumerator loadAvatarsCoroutine() {
    while (true) {
      if (avartarLoadQueue.Count > 0) {
        yield return loadFriendAvatar(avartarLoadQueue.Dequeue());
      } else {
        yield return new WaitForSeconds(1);
      }
    }
  }

  // Because Google's impl takes time to load image, wait until it is not null.
  IEnumerator loadFriendAvatar(IUserProfile profile) {
    Debug.Log("SocialDataCache: Loading avatar of user " + profile.userName);
    userIdToAvatarCache.Add(profile.id, null);
    while (profile.image == null) {
      yield return new WaitForSeconds(500);
      Debug.Log("Not loaded yet! " + profile.userName);
    }
    Debug.Log("SocialDataCache: Finished loading avatar of user " + profile.userName);
    userIdToAvatarCache[profile.id] = createFriendAvatarSprite(profile.image);
  }

  public Sprite createFriendAvatarSprite(Texture2D avatar) {
    return Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(0.5f, 0.5f));
  }

  void backupAvatarImage() {

  }

}
