using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {
  public bool conditionNotRequired;
  public string[] questStartConditions;

  public string description;
  public int numbersToComplete;
  public int goldenCubesWhenComplete = 100;

  // 활성화한 오브젝트 기준
  public bool isAvailable() {
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

  // 구매한 오브젝트 기준
  // public bool isAvailable() {
  //   if ((int)GameController.control.quests[name] != -1) return false; // 해당 퀘스트 진행중

  //   if (conditionNotRequired) return true;

  //   bool andCondition = true;
  //   foreach (string objects in questStartConditions) {
  //     bool orCondition = false;
  //     foreach (string obj in objects.Split(' ')) {
  //       if ((bool) GameController.control.objects[obj]) {
  //         orCondition = true;
  //         break;
  //       }
  //     }
  //     if (!orCondition) {
  //       andCondition = false;
  //       break;
  //     }
  //   }

  //   return andCondition;
  // }
}
