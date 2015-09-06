using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class OnGoingQuest : MonoBehaviour {
  private string questName;
  private GameObject goldenCubes;
  private Text about;
  private Text numbers;
  private int count = 0;
  private int numbersToComplete;
  private string numbersToCompleteToString;
  private int goldenCubesWhenComplete;
  private bool countByTime;
  private bool tutorial;

  private bool convetToInactive = false;
  private float stayCount = 0;
  private float timeCount = 0;

  private float hideStayCount = 0;
  private BeforeIdle beforeIdle;

  private float movingTopStayCount = 0;
  private float completeStayCount = 0;
  private Color color;
  private Color inactiveColor;
  private Color activeColor;
  private bool isComplete = false;
  private bool movingTop = false;
  private float positionY;
  private float positionX;
  private float distance;
  private Vector2 anchorPos;
  private float scaleFactor;
  private int emphasizeStatus = 0;
  private bool gameEnded = false;

  private bool soundPlayed = false;

	public void startQuest(Quest quest, int currentCount) {
    about = transform.Find("About").GetComponent<Text>();
    numbers = transform.Find("Numbers").GetComponent<Text>();
    goldenCubes = transform.Find("GoldenCubes").gameObject;
    beforeIdle = GameObject.Find("BeforeIdle").GetComponent<BeforeIdle>();

    questName = quest.name;
    countByTime = quest.countByTime;
    goldenCubesWhenComplete = quest.goldenCubesWhenComplete;
    tutorial = quest.tutorial;

    about.text = "임무: " + quest.description;
    count = currentCount;
    numbersToComplete = quest.numbersToComplete;
    numbersToCompleteToString = "/" + numbersToComplete.ToString();
    numbers.text = count.ToString() + numbersToCompleteToString;

    inactiveColor = QuestManager.qm.inactiveQuestColor;
    activeColor = new Color (1, 1, 1, 1);
    color = activeColor;

    scaleFactor = transform.localScale.x;
    anchorPos = GetComponent<RectTransform>().anchoredPosition;

    goldenCubes.GetComponent<RectTransform>().anchoredPosition -= new Vector2(about.preferredWidth + 20, 0);
    int rewardCount = goldenCubesWhenComplete;
    foreach (Transform tr in goldenCubes.transform) {
      if (rewardCount > 0) {
        rewardCount--;
        tr.gameObject.SetActive(true);
      } else {
        tr.gameObject.SetActive(false);
      }
    }

    float totalWidth = about.preferredWidth + numbers.GetComponent<RectTransform>().anchoredPosition.x + numbers.preferredWidth;
    positionX = (about.preferredWidth + 15 * goldenCubesWhenComplete - totalWidth / 2);
    anchorPos.x = positionX * scaleFactor;
    GetComponent<RectTransform>().anchoredPosition = anchorPos;

    positionY = anchorPos.y;
    distance = QuestManager.qm.qMoveToY - anchorPos.y;

    goldenCubes.SetActive(false);
    about.gameObject.SetActive(false);
    numbers.gameObject.SetActive(false);
    movingTop = true;
  }

  void Update() {
    if (movingTop) {
      if (hideStayCount < beforeIdle.movingDuration) {
        hideStayCount += Time.deltaTime;
      } else if (movingTopStayCount < QuestManager.qm.qMoveStay) {
        if (!soundPlayed) {
          soundPlayed = true;
          QuestManager.qm.questStartSound.Play();
        }

        goldenCubes.SetActive(true);
        about.gameObject.SetActive(true);
        numbers.gameObject.SetActive(true);
        movingTopStayCount += Time.deltaTime;
      } else {
        positionY = Mathf.MoveTowards(positionY, QuestManager.qm.qMoveToY, Time.deltaTime / QuestManager.qm.qMoveDuring * distance);
        anchorPos.y = positionY;

        scaleFactor = Mathf.MoveTowards(scaleFactor, 1, Time.deltaTime / QuestManager.qm.qMoveDuring * 0.5f);
        transform.localScale = Vector3.one * scaleFactor;
        anchorPos.x = positionX * scaleFactor;

        GetComponent<RectTransform>().anchoredPosition = anchorPos;
      }

      if (positionY == QuestManager.qm.qMoveToY) {
        movingTop = false;
        convetToInactive = true;
      }
    }

    if (convetToInactive) {
      if (stayCount < QuestManager.qm.showQuestDuration) {
        stayCount += Time.deltaTime;
      } else {
        color.r = Mathf.MoveTowards(color.r, inactiveColor.r, Time.deltaTime / QuestManager.qm.hideQuestAlong);
        color.g = Mathf.MoveTowards(color.g, inactiveColor.g, Time.deltaTime / QuestManager.qm.hideQuestAlong);
        color.b = Mathf.MoveTowards(color.b, inactiveColor.b, Time.deltaTime / QuestManager.qm.hideQuestAlong);
        about.color = color;
        numbers.color = color;
      }

      if (color == inactiveColor) convetToInactive = false;
    }

    if (emphasizeStatus == 1) {
      scaleFactor = Mathf.MoveTowards(scaleFactor, 1.5f, Time.deltaTime / QuestManager.qm.qCompleteScaleChangeDuring * 0.5f);
      transform.localScale = Vector3.one * scaleFactor;

      anchorPos.x = positionX * scaleFactor;
      GetComponent<RectTransform>().anchoredPosition = anchorPos;

      if (scaleFactor == 1.5f) emphasizeStatus = 2;
    } else if (emphasizeStatus == 2) {
      if (completeStayCount < QuestManager.qm.qCompleteScaleChangeStay) {
        completeStayCount += Time.deltaTime;
      } else {
        emphasizeStatus = 3;
      }
    } else if (emphasizeStatus == 3) {
      scaleFactor = Mathf.MoveTowards(scaleFactor, 1, Time.deltaTime / QuestManager.qm.qCompleteScaleChangeDuring * 0.5f);
      transform.localScale = Vector3.one * scaleFactor;

      anchorPos.x = positionX * scaleFactor;
      GetComponent<RectTransform>().anchoredPosition = anchorPos;

      if (scaleFactor == 1) emphasizeStatus = 4;
    }

    if (countByTime && !isComplete) {
      timeCount += Time.deltaTime;
      if (timeCount >= 1) {
        addCount(1);
      }
    }
  }

  public void addCount(int howMany) {
    if (isComplete || gameEnded) return;

    if (howMany == 0) {
      count = 0;
    } else {
      count += howMany;
    }

    if (count > numbersToComplete) count = numbersToComplete;
    numbers.text = count.ToString() + numbersToCompleteToString;
    stayCount = 0;
    timeCount = 0;

    if (count >= numbersToComplete) {
      isComplete = true;
      convetToInactive = false;
      emphasizeStatus = 1;
      about.color = QuestManager.qm.completeQuestColor;
      numbers.color = QuestManager.qm.completeQuestColor;
      color = QuestManager.qm.completeQuestColor;
      QuestManager.qm.questCompleteSound.Play();
      // QuestManager.qm.goldCubeBanner.add(goldenCubesWhenComplete);
    } else {
      convetToInactive = true;
      about.color = activeColor;
      numbers.color = activeColor;
      color = activeColor;
    }
  }

  public new string name() {
    return questName;
  }

  public bool isCompleted() {
    return isComplete;
  }

  public string result() {
    gameEnded = true;
    countByTime = false;

    if (isComplete) {
      GameController.control.lastQuestCompleteAt = DateTime.Now;

      if (tutorial) {
        string tutorialsNotDone = PlayerPrefs.GetString("ObjTutorialsNotDone");
        tutorialsNotDone = tutorialsNotDone.Replace(questName, "").Trim();
        tutorialsNotDone = tutorialsNotDone.Replace("  ", " ").Trim();

        PlayerPrefs.SetString("ObjTutorialsNotDone", tutorialsNotDone);
      }

      if (PlayerPrefs.GetInt("FirstQuestComplete") == 0) {
        PlayerPrefs.SetInt("FirstQuestComplete", 1);
        return "FirstQuestComplete";
      } else {
        return "Complete";
      }
    } else {
      return "Failed";
    }
  }

  public int reward() {
    return goldenCubesWhenComplete;
  }
}
