using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {
  public bool conditionNotRequired;
  public string[] questStartConditions;

  public bool countByTime = false;
  public bool doNotShow = false;
  public bool tutorial = false;
  public string description;
  public int numbersToComplete;
  public int goldenCubesWhenComplete = 1;

  // 활성화한 오브젝트 기준
  public bool isAvailable() {
    if (doNotShow) return false;

    if (conditionNotRequired) return true;

    string activeObjects = PlayerPrefs.GetString("MainObjects").Trim() + " " + PlayerPrefs.GetString("SubObjects").Trim();

    if (activeObjects == "") return false;

    bool andCondition = true;
    foreach (string objects in questStartConditions) {
      bool orCondition = false;
      foreach (string obj in objects.Split(' ')) {
        if (activeObjects.Contains(obj)) {
          orCondition = true;
          break;
        }
      }
      if (!orCondition) {
        andCondition = false;
        break;
      }
    }

    return andCondition;
  }
}
