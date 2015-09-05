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
  public CubesYouHave goldenCubes;
  public AudioSource questStartSound;
  public AudioSource questCompleteSound;
  public GoldCubeBanner goldCubeBanner;

  private Transform questsList;
  private Transform objectTutorials;
  private Transform onGoingQuests;

	void Start() {
    if (qm == null) {
      DontDestroyOnLoad(gameObject);
      qm = this;
      questsList = transform.Find("QuestsList");
      objectTutorials = transform.Find("ObjectTutorials");
      onGoingQuests = transform.Find("OnGoingQuests");
    } else if (qm != this) {
      Destroy(gameObject);
      return;
    }
  }

  public void generateQuest() {
    // if another day, reset first quest reward
    if ((DateTime.Now.Date - ((DateTime)GameController.control.lastQuestCompleteAt).Date).TotalDays > 0) {
      PlayerPrefs.SetInt("FirstQuestComplete", 0);
    }

    // if there is not completed tutorial quest, show it
    string tutorialsNotDone = PlayerPrefs.GetString("ObjTutorialsNotDone");
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
    GameObject onGoingQuest = Instantiate(onGoingQuestPrefab);
    onGoingQuest.transform.SetParent(onGoingQuests, false);
    onGoingQuest.GetComponent<OnGoingQuest>().startQuest(quest, currentCount);
    questStartSound.Play();
  }

  public void startQuestWithName(string questName, int currentCount = 0) {
    startQuest(questsList.Find(questName).GetComponent<Quest>(), currentCount);
  }

  public void addCountToQuest(string questName, int howMany = 1) {
    OnGoingQuest ogq = onGoingQuests.GetChild(0).GetComponent<OnGoingQuest>();
    if (ogq.name() == questName) ogq.addCount(howMany);
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

  public void congraturation(int reward) {
    // 연출
    // goldenCubes.add(reward);

    if (PlayerPrefs.GetInt("FirstQuestComplete") == 0) {
      // 첫 보상 연출
      Debug.Log("first quest reward");
      PlayerPrefs.SetInt("FirstQuestComplete", 1);
      goldenCubes.add(firstQuestReward);
      GameController.control.lastQuestCompleteAt = DateTime.Now;
    }
  }
}
