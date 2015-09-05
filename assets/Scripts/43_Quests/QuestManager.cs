using UnityEngine;
using System.Collections;
using System;

public class QuestManager : MonoBehaviour {
  public static QuestManager qm;
  public GameObject onGoingQuestPrefab;
  public int qMoveToY = -50;
  public float qMoveStay = 0.5f;
  public float qMoveDuring = 0.5f;
  public float qCompleteScaleChangeDuring = 0.3f;
  public float qCompleteScaleChangeStay = 0.5f;
  public float showQuestDuration = 2;
  public float hideQuestAlong = 1;
  public Color inactiveQuestColor;
  public Color completeQuestColor;
  public CubesYouHave goldenCubes;

  private Transform questsList;
  private Transform onGoingQuests;

	void Start() {
    if (qm == null) {
      DontDestroyOnLoad(gameObject);
      qm = this;
      questsList = transform.Find("QuestsList");
      onGoingQuests = transform.Find("OnGoingQuests");
    } else if (qm != this) {
      Destroy(gameObject);
      return;
    }
  }

  public void generateQuest() {
    // if there is ongoing quest, do something else

    Quest[] availableQuests = new Quest[questsList.childCount];
    int count = 0;
    foreach (Transform tr in questsList) {
      Quest quest = tr.GetComponent<Quest>();
      if (quest.isAvailable()) {
        availableQuests[count++] = quest;
      }
    }

    if (count == 0) return;

    startQuest(availableQuests[UnityEngine.Random.Range(0, count)]);
  }

  public void startQuest(Quest quest, int currentCount = 0) {
    GameObject onGoingQuest = Instantiate(onGoingQuestPrefab);
    onGoingQuest.transform.SetParent(onGoingQuests, false);
    onGoingQuest.GetComponent<OnGoingQuest>().startQuest(quest, currentCount);
  }

  public void startQuestWithName(string questName, int currentCount = 0) {
    startQuest(questsList.Find(questName).GetComponent<Quest>(), currentCount);
  }

  public void addCountToQuest(string questName, int howMany = 1) {
    foreach (Transform tr in onGoingQuests) {
      OnGoingQuest ogq = tr.GetComponent<OnGoingQuest>();
      if (ogq.name() == questName) {
        ogq.addCount(howMany);
        break;
      }
    }
  }

  void showOnGoingQuests() {
    foreach (DictionaryEntry quest in GameController.control.quests) {
      if ((int) quest.Value != 0) {
        startQuestWithName((string) quest.Key, (int) quest.Value);
      }
    }
  }

  public void checkQuestComplete() {
    foreach (Transform tr in onGoingQuests) {
      OnGoingQuest ogq = tr.GetComponent<OnGoingQuest>();
      ogq.endGame();
    }
  }

  public bool doingQuest(string questName) {
    foreach (Transform tr in onGoingQuests) {
      OnGoingQuest ogq = tr.GetComponent<OnGoingQuest>();
      if (ogq.name() == questName) {
        return true;
      }
    }
    return false;
  }
}
