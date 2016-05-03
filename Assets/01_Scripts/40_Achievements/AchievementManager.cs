using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementManager {
  public static List<AchievementObject> achievementsToReport = new List<AchievementObject>();
  // Initialize and start queue processing coroutine
  public void init(IAchievement[] loadedAchievements) {
    AchievementConstants.init(loadedAchievements);
  }

  public void progressAchievement (string key, int val) {
    if (AchievementConstants.containsKey(key)) {
      foreach(AchievementObject obj in AchievementConstants.getAchieveList(key)) {
        obj.progress(val);
      }
    }
  }
  public void progressAchievement (string key, float val) {
    if (AchievementConstants.containsKey(key)) {
      foreach(AchievementObject obj in AchievementConstants.getAchieveList(key)) {
        obj.progress(val);
      }
    }
  }
  public void progressAchievement (string key, bool val) {
    if (AchievementConstants.containsKey(key)) {
      foreach(AchievementObject obj in AchievementConstants.getAchieveList(key)) {
        obj.progress(val);
      }
    }
  }
  public void progressAchievement (string key, string val) {
    if (AchievementConstants.containsKey(key)) {
      foreach(AchievementObject obj in AchievementConstants.getAchieveList(key)) {
        obj.progress(val);
      }
    }
  }
  public void progressAchievement (string key, DateTime val) {
    if (AchievementConstants.containsKey(key)) {
      foreach(AchievementObject obj in AchievementConstants.getAchieveList(key)) {
        obj.progress(val);
      }
    }
  }

  public void reportAchievements() {
    // Because LoadAchievements() does not work, just report current progress
    foreach(AchievementObject ach in achievementsToReport) {
      ach.report(0);
    }
    achievementsToReport.Clear();
    /*
    // Load achievements from server, to compare with current progress
    // This is for avoiding report negative progress to the server
    NPBinding.GameServices.LoadAchievements((Achievement[] _achievements, string _error)=>{
      if (_achievements == null)
      {
        Debug.Log("Couldn't load achievement list with error = " + _error);
        return;
      }
      int   _achievementCount = _achievements.Length;
      Debug.Log(string.Format("Successfully loaded achievement list. Count={0}.", _achievementCount));

      Dictionary<string, Achievement> cpnpAchDict = new Dictionary<string, Achievement>();
      for (int _iter = 0; _iter < _achievementCount; _iter++)
      {
        cpnpAchDict.Add(_achievements[_iter].Identifier, _achievements[_iter]);
      }

      foreach(AchievementObject ach in achievementsToReport) {
        ach.report(cpnpAchDict[ach.id].PointsScored);
      }
      achievementsToReport.Clear();
    });
    */

  }

  public void reportAllAchievements(IAchievement[] achievements) {
    // Because LoadAchievements() does not work, just report current progress
    AchievementConstants.init(achievements);
  }

  // Maybe need to move this leaderboard thing to new manager (e.g. leaderboard manager)
  public static string LB_SINGLE = "LB_SINGLE";
  public static string LB_OVERALL = "LB_OVERALL";

  public void reportLeaderboard(string id, int point) {
    if (SocialPlatformManager.isAuthenticated()) {
      Debug.Log("Reporting leaderboard: " + id + ", " + point);
      Social.ReportScore((long)point, SocialPlatformManager.spm.leaderboardInfoMap[id], (bool _success) => {
        if (_success)
          Debug.Log("Successfully reported to the leaderboard: " + id + ", " + point);
        else
          Debug.Log("Failed to report to the leaderboared: " + id + ", " + point);
      });
    }
  }

  public void reportAllLeaderboard() {
    reportLeaderboard(AchievementManager.LB_SINGLE, DataManager.dm.getInt("BestCubes"));
    reportLeaderboard(AchievementManager.LB_OVERALL, DataManager.dm.getInt("TotalCubes"));
  }
}
