using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
  public static AudioManager am;

  public float mainIngameVolume = 0.3f;
  public float mainSmallVolume = 0.05f;
  public float mainOriginalPitch = 1;
  public float mainPitchIncrease = 0.05f;
  // public float powerBoostVolumeOn = 0.5f;
  public float volumeChangeDuration = 0.5f;
  // public float powerBoostStartOn = 5;

  private AudioSource main;
  private float mainVolume;
  private float targetMainVolume;
  private float mainVolumeDiff;
  private bool changeMainVolume = false;

  // private AudioSource powerBoost;
  // private float powerBoostVolume;
  // private float targetPowerBoostVolume;
  // private float powerBoostVolumeDiff;
  // private bool changePowerBoostVolume = false;

	void Awake () {
    if (am != null && am != this) {
      Destroy(gameObject);
      return;
    }
    am = this;
	}

  public void setAudio(string name) {
    if (main != null) main.gameObject.SetActive(false);

    main = transform.Find(name).GetComponent<AudioSource>();

    if (DataManager.dm.getBool("BGMOffSetting")) {
      main.volume = 0;
    } else {
      main.volume = mainSmallVolume;
    }

    main.gameObject.SetActive(true);
    // RhythmManager.rm.transform.Find(name).gameObject.SetActive(true);
  }

  void Update() {
    if (changeMainVolume) {
      mainVolume = Mathf.MoveTowards(mainVolume, targetMainVolume, Time.deltaTime * mainVolumeDiff / volumeChangeDuration);
      main.volume = mainVolume;
      if (mainVolume == targetMainVolume) {
        changeMainVolume = false;
        if (targetMainVolume == 0) main.gameObject.SetActive(false);
      }
    }

    // if (changePowerBoostVolume) {
    //   powerBoostVolume = Mathf.MoveTowards(powerBoostVolume, targetPowerBoostVolume, Time.deltaTime * powerBoostVolumeDiff / volumeChangeDuration);
    //   powerBoost.volume = powerBoostVolume;
    //   if (powerBoostVolume == targetPowerBoostVolume) {
    //     changePowerBoostVolume = false;
    //     if (targetPowerBoostVolume == 0) powerBoost.gameObject.SetActive(false);
    //   }
    // }
  }

  public void startPowerBoost() {
  //   if (DataManager.dm.getBool("BGMOffSetting") || AudioListener.volume == 0) return;

  //   powerBoost.gameObject.SetActive(true);
  //   changeVolume("PowerBoost", "Max");
  //   changeVolume("Main", "Min");
  }

  public void stopPowerBoost() {
  //   if (DataManager.dm.getBool("BGMOffSetting") || AudioListener.volume == 0) return;

  //   main.gameObject.SetActive(true);
  //   main.Play();
  //   changeVolume("PowerBoost", "Min");
  //   changeVolume("Main", "Max");
  }

  public void muteBGM(bool mute) {
    if (mute) main.volume = 0;
    else main.volume = mainSmallVolume;
  }

  public void setPitch(int level) {
    main.pitch = mainOriginalPitch + mainPitchIncrease * level;
  }

  public void changeVolume(string what, string level) {
    if (DataManager.dm.getBool("BGMOffSetting") || AudioListener.volume == 0) return;

    if (what == "Main") {
      changeMainVolume = true;
      mainVolume = main.volume;

      if (level == "Max") {
        targetMainVolume = mainIngameVolume;
      } else if (level == "Small") {
        targetMainVolume = mainSmallVolume;
      } else if (level == "Min") {
        targetMainVolume = 0;
      }

      mainVolumeDiff = Mathf.Abs(targetMainVolume - mainVolume);
    }
    //  else if (what == "PowerBoost") {
    //   changePowerBoostVolume = true;
    //   powerBoostVolume = powerBoost.volume;

    //   if (level == "Max") {
    //     targetPowerBoostVolume = powerBoostVolumeOn;
    //   } else if (level == "Min") {
    //     targetPowerBoostVolume = 0;
    //   }

    //   powerBoostVolumeDiff = Mathf.Abs(targetPowerBoostVolume - powerBoostVolume);
    // }
  }
}
