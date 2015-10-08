using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.NativePlugins;
using System;

public static class AchievementConstants {
  public static Dictionary<string, string> achievementIdDict = new Dictionary<string, string>();
  public static Dictionary<string, AchievementObject> achievements = new Dictionary<string, AchievementObject>();
  public static void init() {
    achievementIdDict.Add("CgkI85jcudQLEAIQEQ", "TotalCubes");
    achievementIdDict.Add("CgkI85jcudQLEAIQEg", "TotalCubes");
    achievementIdDict.Add("CgkI85jcudQLEAIQEw", "TotalCubes");

    achievements.Add("TotalCubes", new AchievementObject("", 0, 10));
    //achievements.Add("TotalCubes", new AchievementObject(20));
    //achievements.Add("TotalCubes", new AchievementObject(30));
  }
}
