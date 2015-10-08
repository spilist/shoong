using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

public class DataManager : MonoBehaviour {
  public static DataManager dm;
  public static GPGSManager gpgsManager;
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

  void Awake() {
    if (dm != null && dm != this) {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    dm = this;
    datapath = Application.persistentDataPath + "/GameData.dat";

    bf = new BinaryFormatter();

    ints = new Dictionary<string, int>();
    floats = new Dictionary<string, float>();
    bools = new Dictionary<string, bool>();
    strings = new Dictionary<string, string>();
    dateTimes = new Dictionary<string, DateTime>();
    
    gpgsManager = GetComponent<GPGSManager>();
    gpgsManager.init();

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
  }

  void reset() {
    Debug.Log("Resetting.");
    File.Delete(datapath);
    ints.Clear();
    floats.Clear();
    bools.Clear();
    strings.Clear();
    dateTimes.Clear();

    ints["BatterySavingSetting"] = 60;
    ints["CurrentCubes"] = resetCube;
    ints["TotalCubes"] = resetCube;
    ints["CurrentGoldenCubes"] = resetGoldenCube;
    ints["TotalGoldenCubes"] = resetGoldenCube;
    ints["NumCharactersHave"] = 1;
    bools["robotcogi"] = true;

    PlayerPrefs.SetString("SelectedCharacter", "robotcogi");
    PlayerPrefs.SetString("ObjectTutorialsNotDone", "");
    PlayerPrefs.SetString("MainObjects", "");
    PlayerPrefs.SetString("SubObjects", "");

    data = new SaveData();
  }

  public void setInt(string id, int value) {
    gpgsManager.am.checkAchievement(id, value);
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
    gpgsManager.am.checkAchievement(id, value);
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
    gpgsManager.am.checkAchievement(id, value);
    bools[id] = value;
  }

  public bool getBool(string id) {
    return bools.ContainsKey(id) ? bools[id] : false;
  }

  public void setString(string id, string value) {
    gpgsManager.am.checkAchievement(id, value);
    strings[id] = value;
  }

  public string getString(string id) {
    return strings.ContainsKey(id) ? strings[id] : "";
  }

  public void setDateTime(string id) {
    gpgsManager.am.checkAchievement(id, DateTime.Now);
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
}

[Serializable]
class SaveData {
  public string ints = "";
  public string floats = "";
  public string bools = "";
  public string strings = "";
  public string dateTimes = "";
}

// cubes_by.Add("part", 0);
// cubes_by.Add("comboPart", 0);
// cubes_by.Add("destroying_obstacle", 0);
// cubes_by.Add("destroying_monster", 0);
// cubes_by.Add("cubeDispenser", 0);

//     num_deaths_by.Add("no_energy", 0);
// num_deaths_by.Add("target_obstacle", 0);
// num_deaths_by.Add("big_obstacle", 0);
// num_deaths_by.Add("blackhole", 0);
// num_deaths_by.Add("monster", 0);

// num_destroys.Add("blackhole", 0);
// num_destroys.Add("monster", 0);
// num_destroys.Add("obstacle", 0);

// num_use_objects.Add("exit_blackhole", 0);
// num_use_objects.Add("rebound_by_blackhole", 0);
// num_use_objects.Add("absorb_monster", 0);
// num_use_objects.Add("ride_weakened_monster", 0);
// num_use_objects.Add("unstoppable", 0);
// num_use_objects.Add("unstoppable_with_absorb", 0);
// num_use_objects.Add("unstoppable_with_monster", 0);
// num_use_objects.Add("unstoppable_with_absorb_and_monster", 0);
// num_use_objects.Add("absorb_with_monster", 0);
// num_use_objects.Add("combopart_maxcombo", 0);
// num_use_objects.Add("cubedispenser_maxcombo", 0);
// num_use_objects.Add("RainbowDonuts", 0);
