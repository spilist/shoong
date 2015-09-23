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
  public int firstQuestReward = 30;
  public Color inactiveQuestColor;
  public Color completeQuestColor;
  public AudioSource questStartSound;
  public AudioSource questCompleteSound;

  public string questResult = "";
  public int questReward = 0;

  private Transform questsList;
  private Transform objectTutorials;
  private Transform onGoingQuests;
  private OnGoingQuest ogq;

	void Start() {
    qm = this;
    questsList = transform.Find("QuestsList");
    objectTutorials = transform.Find("ObjectTutorials");
    onGoingQuests = transform.Find("OnGoingQuests");
  }

  public void generateQuest() {
    // if another day, reset first quest reward
    if ((DateTime.Now.Date - DataManager.dm.getDateTime("LastQuestCompletedAt").Date).TotalDays > 0) {
      DataManager.dm.setBool("FirstQuestComplete", false);
    }

    // if there is not completed tutorial quest, show it
    string tutorialsNotDone = PlayerPrefs.GetString("ObjectTutorialsNotDone");
    if (tutorialsNotDone.Trim() != "") {
      foreach (string obj in tutorialsNotDone.Split(null)) {
        Quest tutorial = objectTutorials.Find(obj.Trim()).GetComponent<Quest>();
        if (tutorial.isAvailable()) {
          startQuest(tutorial);
          return;
        }
      }
    }

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
    ogq = ((GameObject)Instantiate(onGoingQuestPrefab)).GetComponent<OnGoingQuest>();
    ogq.transform.SetParent(onGoingQuests, false);
    ogq.startQuest(quest, currentCount);
  }

  public void startQuestWithName(string questName, int currentCount = 0) {
    startQuest(questsList.Find(questName).GetComponent<Quest>(), currentCount);
  }

  public void addCountToQuest(string questName, int howMany = 1) {
    if (ogq.name() == questName) ogq.addCount(howMany);
  }

  public void checkQuestComplete() {
    questResult = ogq.result();
    if (questResult == "Failed") questReward = 0;
    else if (questResult == "Complete") questReward = ogq.reward();
    else if (questResult == "FirstQuestComplete") questReward = firstQuestReward;
  }

  public bool doingQuest(string questName) {
    return ogq.name() == questName;
  }

  public void hideOnGoingQuests() {
    onGoingQuests.gameObject.SetActive(false);
  }
}
