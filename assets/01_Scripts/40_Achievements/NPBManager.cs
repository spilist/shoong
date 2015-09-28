using UnityEngine;
using System.Collections;

// Refered http://blogs.voxelbusters.com/products/cross-platform-native-plugins/game-services
using System.Collections.Generic;
using System;

public delegate void Task();

public class NPBManager : MonoBehaviour {
  public const int TYPE_INT = 0;
  public const int TYPE_FLOAT = 1;
  public const int TYPE_BOOL = 2;
  public const int TYPE_STRING = 3;
  public const int TYPE_DATETIME = 4;

  bool isAuthenticating = false;
  // referred for queue tasks http://answers.unity3d.com/questions/200176/multithreading-and-messaging.html
  private Queue<Task> checkQueue;
  private object _queueLock;
  private bool isProcAchieveCheckRunning;

  public void init() {
    // Initialize queues
    checkQueue = new Queue<Task>();
    _queueLock = new System.Object();

    // Activate processors
    isProcAchieveCheckRunning = true;
    StartCoroutine("procAchieveCheck");
  }
  
  public void authenticate(System.Action<bool> onCompletion) {
    // Authenticate to GPGS
    if (NPBinding.GameServices.LocalUser.IsAuthenticated == false &&
        isAuthenticating == false) {
      isAuthenticating = true;
      NPBinding.GameServices.LocalUser.Authenticate((bool _success)=>{        
        if (_success) {
          Debug.Log("Sign-In Successfully");
          Debug.Log("Local User Details : " + NPBinding.GameServices.LocalUser.ToString());
        } else {
          Debug.Log("Sign-In Failed");
        }
        isAuthenticating = false;
        onCompletion(_success);
      });
    }
  }
    
  public void queueAchieveCheck(string key, System.Object value, int type) {
    lock (_queueLock) {
      checkQueue.Enqueue(()=>doAchieveCheck(key, value, type));
    }
  }

  private void doAchieveCheck(string key, System.Object value, int type) {
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

}