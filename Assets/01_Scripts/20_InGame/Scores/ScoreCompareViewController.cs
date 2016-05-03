using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System.Linq;
using System.Collections.Generic;
using System;

public class ScoreCompareViewController : MonoBehaviour {
  public Image friendAvatar;
  public Text friendName;
  public Text friendScoreComp;
  public float changeOffset;
  public AudioSource[] soundEffects;
  public bool testMode;
  Animation anim;
  ILeaderboard lb;
  IScore[] scores;
  int currentIndex;
  float currScore;
  bool friendScoreLoaded = false;
  bool changingToNextFriend = false;

  void Awake() {
  }

	// Use this for initialization
	void Start () {
    friendScoreLoaded = false;
    anim = GetComponent<Animation>();
    setVisible(false);
#if UNITY_EDITOR
    testMode = true;
#endif
    if (testMode) {
      // Test values for Unity Editor
      IScore[] testScores = new IScore[0];
      for (int i = 0; i < 4; i++) {
        testScores[i] = new IScoreTestImpl(50 * i, i + 1, "TEST_USER_" + i);
      }
      scores = testScores;
      SocialPlatformManager.cache.myProfile = new IUserProfileTestImpl("TESTMYSELF", null, false, "TEST_MYSELF!!!");
      friendScoreLoaded = true;
    } else {
      if (SocialPlatformManager.isAuthenticated()) {
        Social.LoadScores(SocialPlatformManager.spm.leaderboardInfoMap[AchievementManager.LB_SINGLE], loadFriendScores);
        /*
        lb = Social.CreateLeaderboard();
        lb.id = SocialPlatformManager.spm.leaderboardInfoMap[AchievementManager.LB_SINGLE];
        //lb.range = new Range(0, 0);
        //lb.userScope = UserScope.Global;
        lb.LoadScores(loadFriendScores);
        */
        currentIndex = 0;
      }
    }
  }
	
	// Update is called once per frame
	void Update () {
  }
  

  // For using Social.LoadScores
  public void loadFriendScores(IScore[] scores) {
    if (scores.Length == 0)
      return;
    // Remove duplicate and 0 scores, then order score as acending.
    this.scores = scores.GroupBy(x => x.value)
                   .Select(g => g.First())
                   .Where(x => (x.value != 0))
                   .OrderBy(x => x.value)
                   .ToArray();
    Debug.Log("Unique score counts: " + this.scores.Length);
    friendScoreLoaded = true;
  }

  // For using Social.Leaderboard
  public void loadFriendScores(bool success) {
    if (!success || lb.scores.Length == 0)
      return;
    // Remove duplicate and 0 scores, then order score as acending.
    scores = lb.scores.GroupBy(x => x.value)
                   .Select(g => g.First())
                   .Where(x => (x.value != 0))
                   .OrderBy(x => x.value)
                   .ToArray();
    Debug.Log("Unique score counts: " + scores.Length);
    friendScoreLoaded = true;
  }

  public void updateScore(float currScore) {
    // If friend list is not loaded, just do nothing
    if (!friendScoreLoaded)
      return;
    // Only visible when friend score is loaded
    if (!isVisible()) {
      if (scores.Length > 0)
        changeToFriend(scores[currentIndex].userID); // Init to lowest friend!
      else
        changeToFriend(SocialPlatformManager.cache.myProfile.id); // Init to lowest friend!
      setVisible(true);
    }

    // This is for changeToFriend() Coroutine
    this.currScore = currScore;

    if (currentIndex < scores.Length) { // When showing friend
      friendScoreComp.text = (int)(currScore - scores[currentIndex].value) + "";
    } else { // When I'm the highest
      int highScore = DataManager.dm.getInt("BestCubes");
      // TODO: need to load my actual highscore from leaderboard, not cached one
      if (currScore < highScore)
        friendScoreComp.text = (int)(currScore - highScore) + "";
      else
        friendScoreComp.text = "+" + (int)(highScore - currScore) + "";
    }
    // Friend index change routine should be done one by one
    if (changingToNextFriend) {
      return;
    }
    if (currentIndex < scores.Length - 1) { // There are friends left
      if (currScore >= scores[currentIndex].value - changeOffset) {
        StartCoroutine(changeToFriend(scores[currentIndex].userID, changeOffset));
      }
    } else if (currentIndex == scores.Length - 1) { // I'm the highest!
      StartCoroutine(changeToFriend(SocialPlatformManager.cache.myProfile.id, 0));
    } else {
      // do nothing
    }
  }

  public IEnumerator changeToFriend(string userId, float offset) {
    changingToNextFriend = true;    
    startAnimation("ScoreCompareViewPopping", true);
    while (currScore < scores[currentIndex].value) {
      yield return null;
    }
    startAnimation("ScoreCompareViewChanging", false);
    changeToFriend(userId);
    currentIndex++;
    yield return new WaitForSeconds(anim.clip.length);
    changingToNextFriend = false;
    yield return null;
  }

  public void changeToFriend(string userId) {
    if (SocialPlatformManager.cache.userIdToProfileCache.ContainsKey(userId)) {
      friendName.text = SocialPlatformManager.cache.userIdToProfileCache[userId].userName;
      Texture2D avatar = SocialPlatformManager.cache.userIdToProfileCache[userId].image;
      if (avatar != null) {
        friendAvatar.sprite = SocialPlatformManager.cache.createFriendAvatarSprite(avatar);
      }
    } else {
      friendName.text = userId;
    }
  }


  void startAnimation(string name, bool onBeat) {
    RhythmManager.rm.unregisterCallback(GetInstanceID());
    anim.Stop();
    anim.clip = anim.GetClip(name);
    if (onBeat) {
      RhythmManager.rm.registerCallback(GetInstanceID(), () => {
        anim.Play();
      });
    } else {
      anim.Play();
    }
  }

  Sprite createFriendAvatarSprite(Texture2D avatar) {
    return Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(0.5f, 0.5f));
  }

  public void setVisible(bool visibility) {
    GetComponent<Canvas>().enabled = visibility;
  }
  
  public bool isVisible() {
    return GetComponent<Canvas>().enabled;
  }
}
