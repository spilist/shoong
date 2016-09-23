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
    //pausedImage = pauseStatus.transform.Find("PausedImage").gameObject;
    pausedImage = pauseStatus.transform.Find("ResumeButton").gameObject;
    resumingText = pauseStatus.transform.Find("ResumingText").GetComponent<Text>();
    playTouchSound = false;
  }

  override public void activateSelf() {
    if (!gameObject.activeSelf || resuming || ScoreManager.sm.isGameOver()) return;

    activatePause();
    paused = true;
    AudioListener.pause = true;
    pauseFilter.SetActive(true);
    pauseStatus.SetActive(true);
    pausedImage.SetActive(true);
  }

  public void activatePause() {
    Camera.main.GetComponent<CameraMover>().setPaused(true);
    Player.pl.paused = true;
    Time.timeScale = 0;
    RhythmManager.rm.stopBeat();
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
    resumeNow();
  }

  public void resumeNow() {
    activateResume();
    paused = false;
    pauseFilter.SetActive(false);
    pauseStatus.SetActive(false);
    resumingText.gameObject.SetActive(false);
    resumingText.text = "3";
    resuming = false;
    AudioListener.pause = false;
  }

  public void activateResume() {
    RhythmManager.rm.stopBeat(true);
    Camera.main.GetComponent<CameraMover>().setPaused(false);
    Player.pl.paused = false;
    Time.timeScale = 1;
  }

  public bool isPaused() {
    return paused;
  }

  public bool isResuming() {
    return resuming;
  }

  void OnApplicationPause() {
    activateSelf();
  }

  void OnPointerDown() {
    activateSelf();
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
