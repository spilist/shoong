using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
  public static AudioManager am;

  public float mainIngameVolume = 0.3f;
  public float mainSmallVolume = 0.05f;
  public float powerBoostVolumeOn = 0.5f;
  public float volumeChangeDuration = 0.5f;

  private AudioSource main;
  private float mainVolume;
  private float targetMainVolume;
  private float mainVolumeDiff;
  private bool changeMainVolume = false;

  private AudioSource powerBoost;
  private float powerBoostVolume;
  private float targetPowerBoostVolume;
  private float powerBoostVolumeDiff;
  private bool changePowerBoostVolume = false;


	void Awake () {
    if (am != null && am != this) {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    am = this;

    main = transform.Find("Main").GetComponent<AudioSource>();
    powerBoost = transform.Find("PowerBoost").GetComponent<AudioSource>();

    main.volume = mainSmallVolume;
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

    if (changePowerBoostVolume) {
      powerBoostVolume = Mathf.MoveTowards(powerBoostVolume, targetPowerBoostVolume, Time.deltaTime * powerBoostVolumeDiff / volumeChangeDuration);
      powerBoost.volume = powerBoostVolume;
      if (powerBoostVolume == targetPowerBoostVolume) {
        changePowerBoostVolume = false;
        if (targetPowerBoostVolume == 0) powerBoost.gameObject.SetActive(false);
      }
    }
  }

  public void startPowerBoost() {
    powerBoost.gameObject.SetActive(true);
    changeVolume("PowerBoost", "Max");
    changeVolume("Main", "Min");
  }

  public void stopPowerBoost() {
    main.gameObject.SetActive(true);
    changeVolume("PowerBoost", "Min");
    changeVolume("Main", "Max");
  }

  public void changeVolume(string what, string level) {
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
    } else if (what == "PowerBoost") {
      changePowerBoostVolume = true;
      powerBoostVolume = powerBoost.volume;

      if (level == "Max") {
        targetPowerBoostVolume = powerBoostVolumeOn;
      } else if (level == "Min") {
        targetPowerBoostVolume = 0;
      }

      powerBoostVolumeDiff = Mathf.Abs(targetPowerBoostVolume - powerBoostVolume);
    }
  }
}
