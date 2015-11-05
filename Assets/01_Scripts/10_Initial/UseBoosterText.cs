using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UseBoosterText : MonoBehaviour {
  public int limit;
  public TutorialHandler tutoHandler;
  private Text text;
  private int count = 0;

  void Awake() {
    text = GetComponent<Text>();
  }

  public void increment() {
    count++;
    text.text = "USE BOOSTER " + count + "/" + limit;
    if (count == limit) {
      tutoHandler.nextTutorial(3);
    }
  }
}
