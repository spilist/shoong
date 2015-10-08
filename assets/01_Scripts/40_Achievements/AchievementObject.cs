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
  private static float PROGRESS_MAX = 100.00f;

  // Achievement ID in GPGS
  string id;
  public bool notReported = false;
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

  public bool check (int val) {
    notReported = true;
    this.currValInt = val;
    if (this.goalValInt <= val)
      return true;
    else
      return false;
  }
  public bool check (float val) {
    notReported = true;
    this.currValFloat = val;
    if (this.goalValFloat <= val)
      return true;
    else
      return false;
  }
  public bool check (bool val) {    
    notReported = true;
    this.currValBool = val; // not necessary
    if (this.goalValBool == val)
      return true;
    else
      return false;
  }
  public bool check (string val) {   
    notReported = true; 
    this.currValString = val; // not necessary
    if (this.goalValString == val)
      return true;
    else
      return false;
  }
  public bool check (DateTime val) {    
    notReported = true;
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

  public void report() {
    if (notReported == false)
      return;
    float progress = getProgress();
    // When the progress is 0, it just reveals hidden achievement.
    // To avoid accidently revealing achievement, filter it.
    if (progress > 0f) {
      // We should use PlayGamesPlatform.IncrementAchievement, for incremental one,
      // but ReportProgress will work similarlly.
      // The manual does not recommend it, so need to test.
      Social.ReportProgress(id, progress, (bool success) => {
        if (success) {
          Debug.Log(string.Format("Successfully reported points={0} to achievement with ID={1}.", progress, id));
          notReported = false;
        }
        else {
          Debug.Log(string.Format("Failed to report progress points={0} of achievement with ID={0}.", progress, id));
        }
      });
    }
    /*
    NPBinding.GameServices.ReportProgress(id, progress, (bool _status)=>{      
      if (_status) {
        Debug.Log(string.Format("Successfully reported points={0} to achievement with ID={1}.", progress, id));
        notReported = false;
      }
      else {
        Debug.Log(string.Format("Failed to report progress points={0} of achievement with ID={0}.", progress, id));
      }
    });
    */
  }
}
