using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SocialPlatforms;

public static class AchievementConstants {
  private static Dictionary<string, List<AchievementObject>> achievementDict;
  private static List<AchievementObject> allAchievements;

  public static void init(IAchievement[] loadedAchievements) {
    achievementDict = new Dictionary<string, List<AchievementObject>>();
    allAchievements = new List<AchievementObject>();

    // I know that this code for getting current progress of achievement is very dirty. Don't do like this next time :D
    Dictionary<string, IAchievement> loadedAchievementDict = new Dictionary<string, IAchievement>();
    foreach (IAchievement loadedAch in loadedAchievements) {
      loadedAchievementDict.Add(loadedAch.id, loadedAch);
    }
    List<AchievementObject> objList;
    // Beginning of the journey
    // objList = new List<AchievementObject>();
    // objList.Add(new AchievementObject("CgkIubjEkcMWEAIQBQ", false, true));
    // achievements.Add("TutorialDone", objList);
    // Dreamwalker series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("DREAMWALKER_1", 0, 1000, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["DREAMWALKER_1"]].percentCompleted));
    objList.Add(new AchievementObject("DREAMWALKER_2", 0, 2500, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["DREAMWALKER_2"]].percentCompleted));
    objList.Add(new AchievementObject("DREAMWALKER_3", 0, 5000, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["DREAMWALKER_3"]].percentCompleted));
    achievementDict.Add("BestCubes", objList);
    // Toy collector series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("COLLECTOR_1", 0, 5, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["COLLECTOR_1"]].percentCompleted));
    objList.Add(new AchievementObject("COLLECTOR_2", 0, 10, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["COLLECTOR_2"]].percentCompleted));
    objList.Add(new AchievementObject("COLLECTOR_3", 0, 20, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["COLLECTOR_3"]].percentCompleted));
    achievementDict.Add("NumCharactersHave", objList);
    // Traveler series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("TRAVELER_1", 0, 20000, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["TRAVELER_1"]].percentCompleted));
    objList.Add(new AchievementObject("TRAVELER_2", 0, 100000, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["TRAVELER_2"]].percentCompleted));
    objList.Add(new AchievementObject("TRAVELER_3", 0, 1000000, loadedAchievementDict[SocialPlatformManager.spm.achievementInfoMap["TRAVELER_3"]].percentCompleted));
    achievementDict.Add("TotalCubes", objList);
    SocialPlatformManager.spm.am.reportAllLeaderboard();
  }

  public static bool containsKey(string key) {
    return (achievementDict != null && achievementDict.ContainsKey(key));
  }
  public static List<AchievementObject> getAchieveList(string key) {
    return achievementDict[key];
  }

}
