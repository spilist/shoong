using UnityEngine;
using System.Collections;

public class PauseButton : MenusBehavior {
	public GameObject pauseFilter;

  private bool isPaused = false;

  override public void activateSelf() {
    if (isPaused) {

    } else {

    }

    isPaused = !isPaused;
    pauseFilter.SetActive(!pauseFilter.activeSelf);
  }
}
