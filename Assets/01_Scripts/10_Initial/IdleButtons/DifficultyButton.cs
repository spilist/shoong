using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifficultyButton : OnOffButton {
  private string onString = "HARD";
  private string offString = "EASY";
  private Text text;

  override public void initializeRest() {
    text = transform.Find("Text").GetComponent<Text>();
  }

  override public void applyStatus() {
    base.applyStatus();
    EnergyManager.em.setDifficulty(isDifficult());
    RhythmManager.rm.setDifficulty(isDifficult());
    CubeManager.cm.setDifficulty(isDifficult());
    if (clicked) {
      text.text = onString;
    } else {
      text.text = offString;
    }
  }

  public bool isDifficult() {
    return clicked;
  }
}
