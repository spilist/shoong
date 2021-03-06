using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
#if !NO_GPGS
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class AchievementManager {
  public static List<AchievementObject> achievementsToReport = new List<AchievementObject>();
  // Initialize and start queue processing coroutine
  /*
  public void init(IAchievement[] loadedAchievements) {
    AchievementConstants.init(loadedAchievements);
  }
  */

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
