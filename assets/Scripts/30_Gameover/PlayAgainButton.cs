using UnityEngine;
using System.Collections;

public class PlayAgainButton : MenusBehavior {

	override public void activateSelf() {
    Application.LoadLevel(Application.loadedLevel);
  }
}
