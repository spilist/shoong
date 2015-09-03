using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour {
  public static QuestManager questManager;
  public Quest[] quests;

	void OnEnable() {
    if (questManager == null) {
      DontDestroyOnLoad(gameObject);
      questManager = this;
    } else if (questManager != this) {
      Destroy(gameObject);
    }
  }

  void generateQuest() {

  }
}
