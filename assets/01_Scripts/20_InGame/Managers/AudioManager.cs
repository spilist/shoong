using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
  public static AudioManager am;

  public float mainIngameVolume = 0.3f;
  public float mainSmallVolume = 0.05f;
  public float powerBoostTargetVolume = 0.8f;
  public float volumeChangeDuration = 0.5f;

  private AudioSource main;
  private AudioSource powerBoost;
  private bool changeMainVolume = false;
  private float mainVolume;
  private float targetMainVolume;
  private float diff;

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
      mainVolume = Mathf.MoveTowards(mainVolume, targetMainVolume, Time.deltaTime * diff / volumeChangeDuration);
      main.volume = mainVolume;
      if (mainVolume == targetMainVolume) changeMainVolume = false;
    }
  }

  public void changeVolume(string what, string level) {
    if (what == "Main") {
      changeMainVolume = true;
      mainVolume = main.volume;

      if (level == "Max") {
        targetMainVolume = mainIngameVolume;
      } else if (level == "Small") {
        targetMainVolume = mainSmallVolume;
      } else {
        targetMainVolume = 0;
      }

      diff = Mathf.Abs(targetMainVolume - mainVolume);
    }
  }
}
