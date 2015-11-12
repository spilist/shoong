using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.NativePlugins;
using System;

public static class AchievementConstants {
  private static Dictionary<string, List<AchievementObject>> achievements;
  public static void init() {
    achievements = new Dictionary<string, List<AchievementObject>>();
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
    achievements.Add("LastTotalCubes", objList);
    // Toy collector series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQBA", 0, 5));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCQ", 0, 10));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCg", 0, 20));
    achievements.Add("NumCharactersHave", objList);
    // Traveler series
    objList = new List<AchievementObject>();
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQCw", 0, 20000));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQDA", 0, 100000));
    objList.Add(new AchievementObject("CgkIubjEkcMWEAIQDQ", 0, 1000000));
    achievements.Add("TotalCubes", objList);
  }

  public static bool containsKey(string key) {
    return achievements.ContainsKey(key);
  }
  public static List<AchievementObject> getAchieveList(string key) {
    return achievements[key];
  }

}
