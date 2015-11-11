using UnityEngine;
using System.Collections;

public class Tip : MonoBehaviour {
  public bool doNotShow;
  public bool conditionNotRequired;
  public string[] showConditions;

  public string description;

	public bool isAvailable() {
    if (doNotShow) return false;

    if (conditionNotRequired) return true;

    bool andCondition = true;
    foreach (string objects in showConditions) {
      bool orCondition = false;
      foreach (string obj in objects.Split(' ')) {
        if (DataManager.dm.getBool(obj)) {
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
