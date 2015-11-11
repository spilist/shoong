using UnityEngine;
using System.Collections;

public class OnOffBGMButton : OnOffButton {

	override public void activateSelf() {
    base.activateSelf();

    AudioManager.am.muteBGM(clicked);
  }
}
