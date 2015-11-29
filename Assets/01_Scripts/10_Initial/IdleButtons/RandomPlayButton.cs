using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RandomPlayButton : OnOffButton {
  private bool isStarted;
  private AudioSource audioSource;
  private bool audioEnabled;
  private int dailyRandomAvailable;
  public Text randomCountText;
  public Text timeText;

  public GameObject activeObjects;
  public GameObject inactiveObjects;

  override public void initializeRest() {
    audioSource = GetComponent<AudioSource>();

    if (DataManager.dm.isAnotherDay("LastRandomPlayTime")) {
      DataManager.dm.setInt(settingName + "Available", 3);
    }

    dailyRandomAvailable = DataManager.dm.getInt(settingName + "Available");
    randomCountText.text = dailyRandomAvailable + "/3";
  }

  void OnEnable() {
    if (clicked && audioEnabled && dailyRandomAvailable > 0) audioSource.Play();
  }

  override public void applyStatus() {
    if (dailyRandomAvailable > 0) {
      activeObjects.SetActive(true);
      inactiveObjects.SetActive(false);

      if (clicked) {
        if (audioEnabled) audioSource.Play();
        CharacterManager.cm.startRandom();
      } else {
        audioSource.Stop();
        CharacterManager.cm.startRandom(false);
        CharacterManager.cm.setMesh(PlayerPrefs.GetString("SelectedCharacter"));
      }
    } else {
      activeObjects.SetActive(false);
      inactiveObjects.SetActive(true);

      clicked = false;
      GetComponent<Collider>().enabled = false;
    }

    base.applyStatus();
  }

  string timeUntilAvailable() {
    TimeSpan interval = DateTime.Today.AddDays(1) - DateTime.Now;
    return interval.Hours.ToString("00") + ":" + interval.Minutes.ToString("00") + ":" + interval.Seconds.ToString("00");
  }

  void Update() {
    if (dailyRandomAvailable == 0) {
      timeText.text = timeUntilAvailable();
    }
  }

  public void enableAudio() {
    audioEnabled = true;
    if (clicked && dailyRandomAvailable > 0) audioSource.Play();
  }
}
