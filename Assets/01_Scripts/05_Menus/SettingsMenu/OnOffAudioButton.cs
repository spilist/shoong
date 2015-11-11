using UnityEngine;
using System.Collections;

public class OnOffAudioButton : OnOffButton {

	override public void activateSelf() {
    base.activateSelf();

    if (clicked) AudioListener.volume = 0;
    else AudioListener.volume = 1;
  }
}
