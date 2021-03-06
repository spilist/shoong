﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// using UnityEngine.Advertisements;
using Heyzap;

public class DataManager : MonoBehaviour {
  public static DataManager dm;
  private string datapath;

  private Dictionary<string, int> ints;
  private Dictionary<string, float> floats;
  private Dictionary<string, bool> bools;
  private Dictionary<string, string> strings;
  private Dictionary<string, DateTime> dateTimes;

  private SaveData data;
  private BinaryFormatter bf;

  public bool resetAll = false;
  public int resetCube = 0;
  public int resetGoldenCube = 0;

  public int normalFrameRate = 60;
  public bool isBonusStage;

  void Awake() {
    if (dm != null && dm != this) {
      Destroy(gameObject);
      return;
    }

    if (Application.platform == RuntimePlatform.IPhonePlayer) {
      Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
    }

    DontDestroyOnLoad(gameObject);
    dm = this;
    datapath = getInternalPath() + "/GameData.dat";
    Debug.Log("Datapath: " + datapath);

    bf = new BinaryFormatter();

    ints = new Dictionary<string, int>();
    floats = new Dictionary<string, float>();
    bools = new Dictionary<string, bool>();
    strings = new Dictionary<string, string>();
    dateTimes = new Dictionary<string, DateTime>();

    // Advertisement.Initialize("72081");
    // HeyzapAds.Start("d772c6e33d0e63212d4350fc7811d507", HeyzapAds.FLAG_NO_OPTIONS);

    initAppsFlyer();


    if (resetAll || !load()) reset();
    initializeAtGameStart();
  }

  void initializeAtGameStart() {

    Application.targetFrameRate = getInt("BatterySavingSetting");

    if (getBool("MuteAudioSetting")) AudioListener.volume = 0;

    if (getDateTime("FirstPlayDate") == DateTime.MinValue) {
      setDateTime("FirstPlayDate");
    }

    DataManager.dm.setInt("ShowCharacterCreateCount", 0);

  }

  public bool isFirstPlay() {
    return bools["FirstPlay"];
  }

  public bool stillInFirstPlay() {
    return bools["StillInFirstPlay"];
  }

  void initAppsFlyer()
  {
    //AppsFlyer.setAppsFlyerKey("PTuYBhA2CFm48vxR6SGRf7");

#if UNITY_IOS
    return;

    AppsFlyer.setAppID ("PTuYBhA2CFm48vxR6SGRf7");

    // For detailed logging
    //AppsFlyer.setIsDebug (true);

    // For getting the conversion data will be triggered on AppsFlyerTrackerCallbacks.cs file
    AppsFlyer.getConversionData ();

    // For testing validate in app purchase (test against Apple's sandbox environment
    //AppsFlyer.setIsSandbox(true);

    AppsFlyer.trackAppLaunch ();

#elif UNITY_ANDROID

    // All Initialization occur in the override activity defined in the mainfest.xml,
    // including the track app launch
    // For your convinence (if your manifest is occupied) you can define AppsFlyer library
    // here, use this commented out code.

    //AppsFlyer.setAppID ("YOUR_ANDROID_PACKAGE_NAME_HERE");
    //AppsFlyer.setIsDebug (true);
    //AppsFlyer.createValidateInAppListener ("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure");
    //AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks","didReceiveConversionData", "didReceiveConversionDataWithError");
    //AppsFlyer.trackAppLaunch ();

#endif
  }

  bool load() {
    if (File.Exists(datapath)) {
      FileStream file = File.Open(datapath, FileMode.Open);
      try {
        data = (SaveData)bf.Deserialize(file);
      } catch {
        Debug.LogWarning("Couldn't deserialize dataFile. Creating new.");
        return false;
      }
      file.Close();

      ints = stringToIntDict(data.ints);
      floats = stringToFloatDict(data.floats);
      bools = stringToBoolDict(data.bools);
      strings = stringToStringDict(data.strings);
      dateTimes = stringToDateTimeDict(data.dateTimes);

      if (bools["FirstPlayFinished"]) {
        bools["FirstPlay"] = false;
      }

      return true;
    } else {
      Debug.Log ("No data found, creating new.");
      return false;
    }
  }

  public void save() {
    data.ints = intDictToString(ints);
    data.floats = floatDictToString(floats);
    data.bools = boolDictToString(bools);
    data.strings = stringDictToString(strings);
    data.dateTimes = dateTimeDictToString(dateTimes);

    FileStream file = File.Create(datapath);
    bf.Serialize(file, data);
    file.Close();

    // Report achievements when saving data (this is for toy collections achievement)
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.reportAchievements();
  }

  void reset() {
    Debug.Log("Resetting.");
    File.Delete(datapath);
    ints.Clear();
    floats.Clear();
    bools.Clear();
    strings.Clear();
    dateTimes.Clear();

    ints["BatterySavingSetting"] = normalFrameRate;
    ints["CurrentCubes"] = resetCube;
    ints["TotalCubes"] = resetCube;
    ints["CurrentGoldenCubes"] = resetGoldenCube;
    ints["TotalGoldenCubes"] = resetGoldenCube;
    ints["NumCharactersHave"] = 1;
    ints["NormalCollectorLevel"] = 1;
#if UNITY_EDITOR
    strings["ControlMethod"] = "Touch";
    // strings["ControlMethod"] = "CenterBigStick";
#else
    strings["ControlMethod"] = "Stick";
#endif
    bools["robotcogi"] = true;
    bools["TutorialDone"] = true;

    bools["FirstPlay"] = true;
    bools["FirstPlayFinished"] = false;
    bools["StillInFirstPlay"] = true;
    bools["FirstGiftReceived"] = false;

    // By the implementation of OnOffButton, 'true' actually means 'not logged in'
    bools["GoogleLoggedInSetting"] = true;

    PlayerPrefs.SetString("SelectedCharacter", "robotcogi");

    data = new SaveData();
  }

  public void setInt(string id, int value) {
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.progressAchievement(id, value);
    ints[id] = value;
  }

  public int getInt(string id) {
    return ints.ContainsKey(id) ? ints[id] : 0;
  }

  public void increment(string id, int value = 1) {
    setInt(id, ints.ContainsKey(id) ? ints[id] + value : value);
  }

  public void setBestInt(string id, int value) {
    if (value > getInt(id)) setInt(id, value);
  }

  public void setFloat(string id, float value) {
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.progressAchievement(id, value);
    floats[id] = value;
  }

  public void setAverage(string id, string child) {
    setFloat(id, getInt(child) / (float) ints["TotalNumPlays"]);
  }

  public void setBestFloat(string id, float value) {
    if (value > getFloat(id)) setFloat(id, value);
  }

  public float getFloat(string id) {
    return floats.ContainsKey(id) ? floats[id] : 0;
  }

  public void setBool(string id, bool value) {
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.progressAchievement(id, value);
    bools[id] = value;
  }

  public bool getBool(string id) {
    return bools.ContainsKey(id) ? bools[id] : false;
  }

  public void setString(string id, string value) {
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.progressAchievement(id, value);
    strings[id] = value;
  }

  public string getString(string id) {
    return strings.ContainsKey(id) ? strings[id] : "";
  }

  public void setDateTime(string id) {
    if (SocialPlatformManager.spm != null)
      SocialPlatformManager.spm.am.progressAchievement(id, DateTime.Now);
    setDateTime(id, DateTime.Now);
  }

  public void setDateTime(string id, DateTime value) {
    dateTimes[id] = value;
  }

  public DateTime getDateTime(string id) {
    return dateTimes.ContainsKey(id) ? dateTimes[id] : DateTime.MinValue;
  }

  public bool isAnotherDay(string id) {
    return (DateTime.Now.Date - getDateTime(id).Date).TotalDays > 0;
  }

  private Dictionary<string, int> stringToIntDict(string s) {
    Dictionary<string, int> dict = new Dictionary<string, int>();

    if (s == null) {
      Debug.LogWarning("Could not create dictionary, because string is null!");
      return dict;
    }

    string[] tokens = s.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0; i < tokens.Length; i += 2) {
      dict.Add(tokens[i], int.Parse(tokens[i + 1]));
    }

    return dict;
  }

  private Dictionary<string, float> stringToFloatDict(string s) {
    Dictionary<string, float> dict = new Dictionary<string, float>();

    if (s == null) {
      Debug.LogWarning("Could not create dictionary, because string is null!");
      return dict;
    }

    string[] tokens = s.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0; i < tokens.Length; i += 2) {
      dict.Add(tokens[i], float.Parse(tokens[i + 1]));
    }

    return dict;
  }

  private Dictionary<string, bool> stringToBoolDict(string s) {
    Dictionary<string, bool> dict = new Dictionary<string, bool>();

    if (s == null) {
      Debug.LogWarning("Could not create dictionary, because string is null!");
      return dict;
    }

    string[] tokens = s.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0; i < tokens.Length; i += 2) {
      dict.Add(tokens[i], bool.Parse(tokens[i + 1]));
    }

    return dict;
  }

  private Dictionary<string, string> stringToStringDict(string s) {
    Dictionary<string, string> dict = new Dictionary<string, string>();

    if (s == null || s == "") {
      Debug.Log("Could not create dictionary, because string is null or empty. Returning empty dict.");
      return dict;
    }

    string[] tokens = s.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
    for (int i = 0; i < tokens.Length; i += 2) {
      if (tokens.Length == 1) {
        dict.Add(tokens[i], "");
      } else {
        dict.Add(tokens[i], tokens[i + 1]);
      }
    }

    return dict;
  }

  private Dictionary<string, DateTime> stringToDateTimeDict(string s) {
    Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();

    if (s == null) {
      Debug.LogWarning("Could not create dictionary, because string is null!");
      return dict;
    }

    string[] tokens = s.Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0; i < tokens.Length; i += 2) {
      dict.Add(tokens[i], DateTime.Parse(tokens[i + 1]));
    }


    return dict;
  }

  private string intDictToString(Dictionary<string, int> d) {
    string result = "";
    foreach (KeyValuePair<string, int> pair in d) {
      result += pair.Key + "=" + pair.Value + ",";
    }
    result = result.TrimEnd(',');

    return result;
  }

  private string floatDictToString(Dictionary<string, float> d) {
    string result = "";
    foreach (KeyValuePair<string, float> pair in d) {
      result += pair.Key + "=" + pair.Value + ",";
    }
    result = result.TrimEnd(',');

    return result;
  }

  private string boolDictToString(Dictionary<string, bool> d) {
    string result = "";
    foreach (KeyValuePair<string, bool> pair in d) {
      result += pair.Key + "=" + pair.Value + ",";
    }
    result = result.TrimEnd(',');

    return result;
  }

  private string dateTimeDictToString(Dictionary<string, DateTime> d) {
    string result = "";
    foreach (KeyValuePair<string, DateTime> pair in d) {
      result += pair.Key + "=" + pair.Value + ",";
    }
    result = result.TrimEnd(',');

    return result;
  }

  private string stringDictToString(Dictionary<string, string> d) {
    string result = "";
    foreach (KeyValuePair<string, string> pair in d) {
      result += pair.Key + "=" + pair.Value + ",";
    }
    result = result.TrimEnd(',');

    return result;
  }
  string getInternalPath() {
    string path = "";
#if UNITY_ANDROID && !UNITY_EDITOR
 try {
  IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
  IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");

  using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
      using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
        IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
        IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
        IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");

        path = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);

        if(path != null) {
            Debug.Log("Got internal path: " + path);
        }
        else {
            Debug.Log("Using fallback path");
            path = "/data/data/*** YOUR PACKAGE NAME ***/files";
        }
      }
  }
}
catch(Exception e) {
  Debug.Log(e.ToString());
}
#else
    path = Application.persistentDataPath;
#endif
    return path;
  }

  void OnDisable() {
    if (bools == null || !bools["StillInFirstPlay"]) return;

    bools["StillInFirstPlay"] = false;
    save();
  }
}

[Serializable]
class SaveData {
  public string ints = "";
  public string floats = "";
  public string bools = "";
  public string strings = "";
  public string dateTimes = "";
}
