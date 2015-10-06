using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {
  public const int TYPE_INT = 0;
  public const int TYPE_FLOAT = 1;
  public const int TYPE_BOOL = 2;
  public const int TYPE_STRING = 3;
  public const int TYPE_DATETIME = 4;
  private Dictionary<string, Achievement> achievements;
  
  // referred for queue tasks http://answers.unity3d.com/questions/200176/multithreading-and-messaging.html
  private Queue<Task> checkQueue;
  private object _queueLock;
  private bool isProcAchieveCheckRunning;

  public void init() {
    // Initialize achievement data
    achievements = new Dictionary<string, Achievement>();
    
    // Initialize achievement processing queue
    checkQueue = new Queue<Task>();
    _queueLock = new System.Object();
    
    // Activate achievement processor
    isProcAchieveCheckRunning = true;
    StartCoroutine("procAchieveCheck");
  }

  
  public void queueAchieveCheck(string key, System.Object value, int type) {
    checkQueue.Enqueue(()=>achieveCheck(key, value, type));
  }
  
  private void achieveCheck(string key, System.Object value, int type) {
    //AchieveConstants.match(key, value, type);
  }
  
  private IEnumerator procAchieveCheck() {
    while (isProcAchieveCheckRunning) {
      lock (_queueLock) {
        if (checkQueue.Count > 0) {
          checkQueue.Dequeue()();
        }
      }
      yield return new WaitForSeconds(1f);
    }
    yield break;
  }

  public void match(string key, System.Object value, int type) {
    try {
      switch (type) {
        case TYPE_INT:
          int valInt = (int) value;
          break;
        case TYPE_FLOAT:
          float valFloat = (float) value;
          break;
        case TYPE_BOOL:
          bool valBool = (bool) value;
          break;
        case TYPE_STRING:
          string valString = (string) value;
          break;
        case TYPE_DATETIME:
          DateTime valDateTime = (DateTime) value;
          break;
        default:
          Debug.LogError("doAchieveCheck: Could not recognize type argument: " + type);
          break;
      }
    } catch (InvalidCastException e) {
      Debug.LogError("Failed to check achievement because of the key/type mismatch: " + e.Message);
    }
  }

  class Achievement {
    string key;
    System.Object goalVal;
    int type;

    Quest nextQuest;

    public Achievement (string key, System.Object val, int type) {
      this.key = key;
      this.goalVal = val;
      this.type = type;
    }
  }
}
