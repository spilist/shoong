﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnGoingQuest : MonoBehaviour {
  private string questName;
  private Text about;
  private Text numbers;
  private int count = 0;
  private int numbersToComplete;
  private string numbersToCompleteToString;
  private int goldenCubesWhenComplete;
  private bool countByTime;

  private bool convetToInactive = false;
  private float stayCount = 0;
  private float timeCount = 0;
  private Color color;
  private Color inactiveColor;
  private Color activeColor;
  private bool isComplete = false;
  private bool movingTop = false;
  private float positionY;
  private float distance;
  private Vector2 anchorPos;
  private float scaleFactor;
  private int emphasizeStatus = 0;

	public void startQuest(Quest quest, int currentCount) {
    questName = quest.name;
    countByTime = quest.countByTime;
    about = transform.Find("About").GetComponent<Text>();
    numbers = transform.Find("Numbers").GetComponent<Text>();

    about.text = "임무: " + quest.description;
    count = currentCount;
    numbersToComplete = quest.numbersToComplete;
    numbersToCompleteToString = "/" + numbersToComplete.ToString();
    numbers.text = count.ToString() + numbersToCompleteToString;

    goldenCubesWhenComplete = quest.goldenCubesWhenComplete;

    inactiveColor = QuestManager.qm.inactiveQuestColor;
    activeColor = new Color (1, 1, 1, 1);
    color = activeColor;

    movingTop = true;
    anchorPos = GetComponent<RectTransform>().anchoredPosition;

    float totalWidth = about.preferredWidth + numbers.GetComponent<RectTransform>().anchoredPosition.x + numbers.preferredWidth;
    anchorPos.x = about.preferredWidth - totalWidth / 2;

    positionY = anchorPos.y;
    distance = QuestManager.qm.qMoveToY - anchorPos.y;
    scaleFactor = transform.localScale.x;
  }

  void Update() {
    if (movingTop) {
      positionY = Mathf.MoveTowards(positionY, QuestManager.qm.qMoveToY, Time.deltaTime / QuestManager.qm.qMoveDuring * distance);
      anchorPos.y = positionY;
      GetComponent<RectTransform>().anchoredPosition = anchorPos;

      scaleFactor = Mathf.MoveTowards(scaleFactor, 1, Time.deltaTime / QuestManager.qm.qMoveDuring * 0.5f);
      transform.localScale = Vector3.one * scaleFactor;

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
      scaleFactor = Mathf.MoveTowards(scaleFactor, 1.5f, Time.deltaTime / 0.5f);
      transform.localScale = Vector3.one * scaleFactor;
      if (scaleFactor == 1.5f) emphasizeStatus = 2;
    } else if (emphasizeStatus == 2) {
      scaleFactor = Mathf.MoveTowards(scaleFactor, 1, Time.deltaTime / 0.5f);
      transform.localScale = Vector3.one * scaleFactor;
      if (scaleFactor == 1) emphasizeStatus = 3;
    } else if (emphasizeStatus == 3) {
      if (stayCount < QuestManager.qm.showQuestDuration) {
        stayCount += Time.deltaTime;
      } else {
        color.a = Mathf.MoveTowards(color.a, 0, Time.deltaTime / QuestManager.qm.hideQuestAlong);
        about.color = color;
        numbers.color = color;
      }

      if (color.a == 0) emphasizeStatus = 0;
    }

    if (countByTime && !isComplete) {
      timeCount += Time.deltaTime;
      if (timeCount >= 1) {
        addCount(1);
      }
    }
  }

  public void addCount(int howMany) {
    if (isComplete) return;

    if (howMany == 0) {
      count = 0;
    } else {
      count += howMany;
    }

    if (count > numbersToComplete) count = numbersToComplete;
    numbers.text = count.ToString() + numbersToCompleteToString;
    GameController.control.quests[questName] = count;
    stayCount = 0;
    timeCount = 0;

    if (count >= numbersToComplete) {
      isComplete = true;
      convetToInactive = false;
      emphasizeStatus = 1;
      about.color = QuestManager.qm.completeQuestColor;
      numbers.color = QuestManager.qm.completeQuestColor;
      color = QuestManager.qm.completeQuestColor;
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

  public void endGame() {
    countByTime = false;

    if (isComplete) {
      QuestManager.qm.goldenCubes.add(goldenCubesWhenComplete);
    } else {
      // if tutorial, don't
      GameController.control.quests[questName] = 0;
    }

    // Destroy(gameObject);
  }
}
