using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.NativePlugins;
using System;

public static class AchievementConstants {
  private static Dictionary<string, List<AchievementObject>> achievementDict;
  private static List<AchievementObject> allAchievements;

  public static void init() {
    achievementDict = new Dictionary<string, List<AchievementObject>>();
    allAchievements = new List<AchievementObject>();
    List<AchievementObject> objList;
    // Beginning of the journey
    // objList = new List<AchievementObject>();
    // objList.Add(new AchievementObject("CgkIubjEkcMWEAIQBQ", false, true));
    // achievements.Add("TutorialDone", objList);
    // Dreamwalker series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQAw", 0, 1000));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQBw", 0, 2500));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCA", 0, 5000));
    foreach (AchievementObject obj in objList) {
      obj.progress(DataManager.dm.getInt("BestCubes"));
      if (NPBinding.GameServices.LocalUser.IsAuthenticated == true)
        obj.report(0);
    }
    achievementDict.Add("BestCubes", objList);
    // Toy collector series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQBA", 0, 5));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCQ", 0, 10));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCg", 0, 20));
    foreach (AchievementObject obj in objList) {
      obj.progress(DataManager.dm.getInt("NumCharactersHave"));
      if (NPBinding.GameServices.LocalUser.IsAuthenticated == true)
        obj.report(0);
    }
    achievementDict.Add("NumCharactersHave", objList);
    // Traveler series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCw", 0, 20000));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQDA", 0, 100000));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQDQ", 0, 1000000));
    foreach (AchievementObject obj in objList) {
      obj.progress(DataManager.dm.getInt("TotalCubes"));
      if (NPBinding.GameServices.LocalUser.IsAuthenticated == true)
        obj.report(0);
    }
    achievementDict.Add("TotalCubes", objList);
  
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == true) {
      DataManager.npbManager.am.reportAllLeaderboard();
    }
  }

  public static bool containsKey(string key) {
    return achievementDict.ContainsKey(key);
  }
  public static List<AchievementObject> getAchieveList(string key) {
    return achievementDict[key];
  }

}
