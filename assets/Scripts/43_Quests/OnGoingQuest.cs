using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnGoingQuest : MonoBehaviour {
  private string questName;
  private Text about;
  private Text numbers;
  private int count = 0;
  private int numbersToComplete;
  private string numbersToCompleteToString;
  private bool startHide = false;
  private float stayCount = 0;
  private Color color;
  private bool isComplete = false;

	public void startQuest(Quest quest, int currentCount) {
    questName = quest.name;
    about = transform.Find("About").GetComponent<Text>();
    numbers = transform.Find("Numbers").GetComponent<Text>();

    about.text = quest.description;
    count = currentCount;
    numbersToComplete = quest.numbersToComplete;
    numbersToCompleteToString = "/" + numbersToComplete.ToString();
    numbers.text = count.ToString() + numbersToCompleteToString;
    color = new Color (1, 1, 1, 1);
  }

  public void addCount(int howMany) {
    if (isComplete) return;

    about.color = new Color (1, 1, 1, 1);
    numbers.color = new Color (1, 1, 1, 1);

    count += howMany;
    if (count > numbersToComplete) count = numbersToComplete;
    numbers.text = count.ToString() + numbersToCompleteToString;
    GameController.control.quests[questName] = count;

    startHide = true;
    stayCount = 0;
    color.a = 1;

    if (count >= numbersToComplete) isComplete = true;
  }

  public new string name() {
    return questName;
  }

  public void hide() {
    about.color = new Color (1, 1, 1, 0);
    numbers.color = new Color (1, 1, 1, 0);
  }

  void Update() {
    if (startHide) {
      if (stayCount < QuestManager.qm.showQuestDuration) {
        stayCount += Time.deltaTime;
      } else {
        color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / QuestManager.qm.hideQuestAlong);
        about.color = color;
        numbers.color = color;
      }

      if (color.a == 0) startHide = false;
    }
  }

  public bool isCompleted() {
    return isComplete;
  }

  public void congraturation() {
    GameController.control.quests[questName] = -1;
    Destroy(gameObject);
  }
}
