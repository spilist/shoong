using System;
using UnityEngine;

/*
 * Class for each achievement.
 * check() returns true when (value >= goal) for int, float, DateTime,
 * and returns true when (value == goal) for bool and string
 */
// Used class name 'AchievementObject' to distinguish with 'Achievement' class of NPBM
public class AchievementObject {
  public enum TYPE {INT, FLOAT, BOOL, STRING, DATETIME};
  private static float PROGRESS_MAX = 10000f;

  // Achievement ID in GPGS
  public string id;
  // Initial Value
  int initValInt; 
  float initValFloat;
  bool initValBool;
  string initValString;
  DateTime initValDateTime;
  // Current value to report progress
  int currValInt;
  float currValFloat;
  bool currValBool;
  string currValString;
  DateTime currValDateTime;
  // Goal Value
  int goalValInt;
  float goalValFloat;
  bool goalValBool;
  string goalValString;
  DateTime goalValDateTime;
  // Value Type
  public TYPE type;
  // Next achievement
  public AchievementObject nextAchievement = null;
  
  public AchievementObject (string id, int initVal, int goalVal) {
    this.id = id;
    this.initValInt = initVal;
    this.goalValInt = goalVal;
    this.type = TYPE.INT;
  }

  public AchievementObject (string id, float initVal, float goalVal) {
    this.id = id;
    this.initValFloat = initVal;
    this.goalValFloat = goalVal;
    this.type = TYPE.FLOAT;
  }

  public AchievementObject (string id, bool initVal, bool goalVal) {
    this.id = id;
    this.initValBool = initVal;
    this.goalValBool = goalVal;
    this.type = TYPE.BOOL;
  }

  public AchievementObject (string id, string initVal, string goalVal) {
    this.id = id;
    this.initValString = initVal;
    this.goalValString = goalVal;
    this.type = TYPE.STRING;
  }

  public AchievementObject (string id, DateTime initVal, DateTime goalVal) {
    this.id = id;
    this.initValDateTime = initVal;
    this.goalValDateTime = goalVal;
    this.type = TYPE.DATETIME;
  }

  public bool progress (int val) {
    if (this.currValInt != val)
      AchievementManager.achievementsToReport.Add(this);
    this.currValInt = val;
    if (this.goalValInt <= val)
      return true;
    else
      return false;
  }
  public bool progress (float val) {
    if (this.currValFloat != val)
      AchievementManager.achievementsToReport.Add(this);
    this.currValFloat = val;
    if (this.goalValFloat <= val)
      return true;
    else
      return false;
  }
  public bool progress (bool val) {  
    if (this.currValBool != val)  
      AchievementManager.achievementsToReport.Add(this);
    this.currValBool = val; // not necessary
    if (this.goalValBool == val)
      return true;
    else
      return false;
  }
  public bool progress (string val) { 
    if (this.currValString != val)     
      AchievementManager.achievementsToReport.Add(this);
    this.currValString = val; // not necessary
    if (this.goalValString == val)
      return true;
    else
      return false;
  }
  public bool progress (DateTime val) { 
    if (this.currValDateTime != val)   
      AchievementManager.achievementsToReport.Add(this);
    this.currValDateTime = val;
    if (DateTime.Compare(this.goalValDateTime, val) <= 0)
      return true;
    else
      return false;
  }

  public float getProgress() {
    float progress, intersect;
    switch(type) {
      case TYPE.INT:
        intersect = ((currValInt - initValInt) < goalValInt ? (currValInt - initValInt) : goalValInt);  
        progress = (float) Math.Floor(intersect / goalValInt * PROGRESS_MAX);
        break;
      case TYPE.FLOAT:
        intersect = ((currValFloat - initValFloat) < goalValFloat ? (currValFloat - initValFloat) : goalValFloat);  
        progress = (float) Math.Floor(intersect / goalValFloat * PROGRESS_MAX);
        break;
      case TYPE.BOOL:
        if (currValBool == goalValBool)
          progress = 1f;
        else
          progress = 0f;
        break;
      case TYPE.STRING:
        if (currValString == goalValString)
          progress = 1f;
        else
          progress = 0f;
        break;
      case TYPE.DATETIME:
        progress = 0f; // TODO: write code considering date and period
        break;
      default:
        progress = 0f;
        break;
    }
    Debug.Log("Reporting progress: " + this.id + ", " + progress);

    return progress;
  }

  public void report(int currProgress = 0) {
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == false)
      return;
    int progress = (int) getProgress();
    if (progress > currProgress) {
      // We should use PlayGamesPlatform.IncrementAchievement, for incremental one,
      // but ReportProgress will work similarlly.
      // The manual does not recommend it, so need to test.
      NPBinding.GameServices.ReportProgressWithID(id, progress, (bool _status, string _error)=>{      
        if (_status) {
          Debug.Log(string.Format("Successfully reported points={0} to achievement with ID={1}.", progress, id));
        }
        else {
          Debug.Log(string.Format("Failed to report progress points={0} of achievement with ID={1}. Error={2}", progress, id, _error));
        }
      });
    }
  }
}
