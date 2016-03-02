using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {
  public BeforeIdle beforeIdle;
  public bool first;

	override public void activateSelf() {
    if (DataManager.dm.isFirstPlay) {
      int firstPlayAgainCount = DataManager.dm.getInt("FirstPlayAgainCount") + 1;
      if (firstPlayAgainCount < 10) {
        TrackingManager.tm.firstPlayLog("8_PlayAgain_" + firstPlayAgainCount);
        DataManager.dm.increment("FirstPlayAgainCount");
      }
    }

    beforeIdle.playAgain();
    AudioManager.am.changeVolume("Main", "Min");
  }
}
