using UnityEngine;
using System.Collections;
using System;

public class QuestManager : MonoBehaviour {
  public static QuestManager qm;
  public GameObject onGoingQuestPrefab;
  public int distBtwOnGoingQuest = 50;
  public float showQuestDuration = 2;
  public float hideQuestAlong = 1;
  public bool resetQuest = false;
  public CubesYouHave goldenCubes;

  private Transform questLists;
  private Transform onGoingQuests;

	void OnEnable() {
    if (resetQuest) {
      resetPrevQuests();
      GameController.control.lastQuestGivenAt = DateTime.MinValue;
    }

    if (qm == null) {
      DontDestroyOnLoad(gameObject);
      qm = this;
      questLists = transform.Find("QuestLists");
      onGoingQuests = transform.Find("OnGoingQuests");
    } else if (qm != this) {
      Destroy(gameObject);
      return;
    }

    generateDailyQuest();
  }

  public void generateDailyQuest() {
    DateTime now = DateTime.Now;

    // if a quest is already given on the day, show it
    if ((now.Date - ((DateTime)GameController.control.lastQuestGivenAt).Date).TotalDays == 0) {
      showOnGoingQuests();
      return;
    }

    // if new day, reset previous quests and generate new one
    resetPrevQuests();
    GameController.control.lastQuestGivenAt = now;

    Quest[] availableQuests = new Quest[questLists.childCount];
    int count = 0;
    foreach (Transform tr in questLists) {
      Quest quest = tr.GetComponent<Quest>();
      if (quest.isAvailable()) {
        availableQuests[count++] = quest;
      }
    }

    if (count == 0) return;

    startQuest(availableQuests[UnityEngine.Random.Range(0, count)]);
  }

  public void startQuest(Quest quest, int currentCount = 0) {
    if ((int) GameController.control.quests[quest.name] == -1) {
      GameController.control.quests[quest.name] = 0;
    };

    GameObject onGoingQuest = Instantiate(onGoingQuestPrefab);
    onGoingQuest.transform.SetParent(onGoingQuests, false);
    onGoingQuest.GetComponent<OnGoingQuest>().startQuest(quest, currentCount);
    onGoingQuest.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, - onGoingQuests.childCount * distBtwOnGoingQuest);
  }

  public void startQuestWithName(string questName, int currentCount = 0) {
    startQuest(questLists.Find(questName).GetComponent<Quest>(), currentCount);
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
      if ((int) quest.Value != -1) {
        startQuestWithName((string) quest.Key, (int) quest.Value);
      }
    }
  }

  public void resetPrevQuests() {
    Hashtable table = new Hashtable();
    foreach (DictionaryEntry quest in GameController.control.quests) {
      table.Add(quest.Key, -1);
    }
    GameController.control.quests = table;
  }

  public void toggleView() {
    foreach (Transform tr in onGoingQuests) {
      tr.GetComponent<OnGoingQuest>().toggleView();
    }
  }

  public void checkQuestComplete() {
    foreach (Transform tr in onGoingQuests) {
      OnGoingQuest ogq = tr.GetComponent<OnGoingQuest>();
      if (ogq.isCompleted()) {
        Debug.Log("You completed a quest");
        ogq.congraturation();
      }
    }
  }
}
