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
  private static int PROGRESS_MAX = 10000;

  // Achievement ID in GPGS
  string id;
  bool notReported = false;
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
    this.initValInt = initVal;
    this.goalValInt = goalVal;
    this.type = TYPE.INT;
  }

  public AchievementObject (string id, float initVal, float goalVal) {
    this.initValFloat = initVal;
    this.goalValFloat = goalVal;
    this.type = TYPE.FLOAT;
  }

  public AchievementObject (string id, bool initVal, bool goalVal) {
    this.initValBool = initVal;
    this.goalValBool = goalVal;
    this.type = TYPE.BOOL;
  }

  public AchievementObject (string id, string initVal, string goalVal) {
    this.initValString = initVal;
    this.goalValString = goalVal;
    this.type = TYPE.STRING;
  }

  public AchievementObject (string id, DateTime initVal, DateTime goalVal) {
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

  public int getProgress() {
    int progress;
    switch(type) {
      case TYPE.INT:
        progress = (int) Math.Floor((float) (currValInt - initValInt) / goalValInt * PROGRESS_MAX);
        break;
      case TYPE.FLOAT:
        progress = (int) Math.Floor((currValFloat - initValFloat) / goalValFloat * PROGRESS_MAX);
        break;
      case TYPE.BOOL:
        if (currValBool == goalValBool)
          progress = 1;
        else
          progress = 0;
        break;
      case TYPE.STRING:
        if (currValString == goalValString)
          progress = 1;
        else
          progress = 0;
        break;
      case TYPE.DATETIME:
        progress = 0; // TODO: write code considering date and period
        break;
      default:
        progress = 0;
        break;
    }
    return progress;
  }

  public void report() {
    if (notReported == false)
      return;
    int progress = getProgress();
    NPBinding.GameServices.ReportProgress(id, progress, (bool _status)=>{      
      if (_status) {
        Debug.Log(string.Format("Successfully reported points={0} to achievement with ID={1}.", progress, id));
        notReported = false;
      }
      else {
        Debug.Log(string.Format("Failed to report progress points={0} of achievement with ID={0}.", progress, id));
      }
    });
  }
}
