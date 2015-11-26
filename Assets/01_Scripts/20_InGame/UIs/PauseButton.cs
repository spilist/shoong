using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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
    playTouchSound = false;
  }

  override public void activateSelf() {
    if (!gameObject.activeSelf || resuming || ScoreManager.sm.isGameOver()) return;

    paused = true;
    Camera.main.GetComponent<CameraMover>().setPaused(true);
    Time.timeScale = 0;
    pauseFilter.SetActive(true);
    pauseStatus.SetActive(true);
    pausedImage.SetActive(true);
    AudioListener.pause = true;
    RhythmManager.rm.stopBeat();
    // For score hack check
    DataValidator.storeIntData("Cubes_totalCount", CubeManager.cm.totalCount);    
    DataValidator.storeIntData("Cubes_pointsByTime", CubeManager.cm.pointsByTime);
    DataValidator.storeIntData("Cubes_goldenCube", DataManager.dm.getInt("CurrentGoldenCubes"));
  }

  public void resume() {
    if (resuming) return;
    resuming = true;
    pausedImage.SetActive(false);
    resumingText.gameObject.SetActive(true);
    // For score hack check
    if (DataValidator.validateIntData("Cubes_totalCount", CubeManager.cm.totalCount) == false ||
        DataValidator.validateIntData("Cubes_pointsByTime", CubeManager.cm.pointsByTime) == false) {
      CubeManager.cm.totalCount = DataValidator.getStoredIntData("Cubes_totalCount");
      CubeManager.cm.pointsByTime = DataValidator.getStoredIntData("Cubes_pointsByTime");
    }
    if (DataValidator.validateIntData("Cubes_goldenCube", DataManager.dm.getInt("CurrentGoldenCubes")) == false) {
      DataManager.dm.setInt("CurrentGoldenCubes", DataValidator.getStoredIntData("Cubes_goldenCube"));
    }
    StartCoroutine("resumeGame");
  }

  IEnumerator resumeGame() {
    int count = 3;
    while (count > 0) {
      resumingText.text = count.ToString();
      yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(1));
      count--;
    }

    RhythmManager.rm.stopBeat(true);
    Camera.main.GetComponent<CameraMover>().setPaused(false);
    pauseFilter.SetActive(false);
    pauseStatus.SetActive(false);
    resumingText.gameObject.SetActive(false);
    resumingText.text = "3";
    paused = false;
    resuming = false;
    Time.timeScale = 1;
    AudioListener.pause = false;
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
