using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButton : MenusBehavior {
	public GameObject pauseFilter;
  public GameObject pauseStatus;

  private bool paused = false;
  private bool resuming = false;
  private GameObject pausedImage;
  private Text resumingText;

  void Start() {
    pausedImage = pauseStatus.transform.Find("PausedImage").gameObject;
    resumingText = pauseStatus.transform.Find("ResumingText").GetComponent<Text>();
  }

  override public void activateSelf() {
    paused = true;
    Time.timeScale = 0;
    pauseFilter.SetActive(true);
    pauseStatus.SetActive(true);
    pausedImage.SetActive(true);
  }

  public void resume() {
    if (resuming) return;
    resuming = true;
    pausedImage.SetActive(false);
    resumingText.gameObject.SetActive(true);
    StartCoroutine("resumeGame");
  }

  IEnumerator resumeGame() {
    int count = 3;
    while (count > 0) {
      resumingText.text = count.ToString();
      yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(1));
      count--;
    }

    pauseFilter.SetActive(false);
    pauseStatus.SetActive(false);
    resumingText.gameObject.SetActive(false);
    resumingText.text = "3";
    paused = false;
    resuming = false;
    Time.timeScale = 1;
  }

  public bool isPaused() {
    return paused;
  }

  void OnApplicationPause() {
    if (gameObject.activeSelf) activateSelf();
  }
}

public static class CoroutineUtilities {
  public static IEnumerator WaitForRealTime(float delay) {
    while(true) {
      float pauseEndTime = Time.realtimeSinceStartup + delay;
      while (Time.realtimeSinceStartup < pauseEndTime) {
        yield return 0;
      }
      break;
    }
  }
}
