using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {
  public BeforeIdle beforeIdle;

	override public void activateSelf() {
    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
  }
}
