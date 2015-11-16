using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {
  public BeforeIdle beforeIdle;
  public bool first;

	override public void activateSelf() {
    if (first) FacebookManager.fb.firstPlayLog("8_PlayAgain");

    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
  }
}
