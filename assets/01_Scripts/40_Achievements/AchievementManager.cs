using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using VoxelBusters.NativePlugins;

public class AchievementManager {  
  // Initialize and start queue processing coroutine
  public void init() {    
  }

  public bool checkAchievement (string key, int val) {
    if (AchievementConstants.achievements.ContainsKey(key))
      return AchievementConstants.achievements[key].check(val);
    return false;
  }
  public bool checkAchievement (string key, float val) {
    if (AchievementConstants.achievements.ContainsKey(key))
      return AchievementConstants.achievements[key].check(val);
    return false;
  }
  public bool checkAchievement (string key, bool val) {   
    if (AchievementConstants.achievements.ContainsKey(key)) 
      return AchievementConstants.achievements[key].check(val);
    return false;
  }
  public bool checkAchievement (string key, string val) {  
    if (AchievementConstants.achievements.ContainsKey(key)) 
      return AchievementConstants.achievements[key].check(val);
    return false;
  }
  public bool checkAchievement (string key, DateTime val) {  
    if (AchievementConstants.achievements.ContainsKey(key))  
      return AchievementConstants.achievements[key].check(val);
    return false;
  }
  
  public void reportAchievements() {
    NPBinding.GameServices.LoadAchievements((Achievement[] _achievements)=>{      
      if (_achievements == null)
      {
        Debug.Log("Couldn't load achievement list.");
        return;
      }
      
      int _achievementCount = _achievements.Length;
      Debug.Log(string.Format("Successfully loaded achievement list. Count={0}.", _achievementCount));
      
      for (int _iter = 0; _iter < _achievementCount; _iter++)
      {
        Achievement achievement = _achievements[_iter];
        Debug.Log(string.Format("[Index {0}]: {1}", _iter, achievement));

        string key = AchievementConstants.achievementIdDict[achievement.Identifier];
        if (AchievementConstants.achievements.ContainsKey(key)) {
          AchievementConstants.achievements[key].report();
        }
      }
    });
  }

}