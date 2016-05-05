using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatSelector : MonoBehaviour {
  BeatCounter[] counters;
  BeatConstants beatConstants;
  public AudioSource currentAudioSource = null;
  private IEnumerator currentMovePitchCoroutine;
  private float currentPitchModifier = 0f;
  BeatSynchronizer beatSynchronizer;
	// Use this for initialization
	void Awake () {
    counters = GetComponents<BeatCounter>();
    beatConstants = GetComponent<BeatConstants>();
    currentAudioSource = GetComponent<AudioSource>();
    beatSynchronizer = GetComponent<BeatSynchronizer>();
    //StartCoroutine(wait_and_go());
  }
	
	// Update is called once per frame
	void Update () {
	
	}

  public void selectBonusSource(int index) {
    counters = GetComponents<BeatCounter>();
    beatConstants = GetComponent<BeatConstants>();
    currentAudioSource = GetComponent<AudioSource>();
    beatSynchronizer = GetComponent<BeatSynchronizer>();
    BeatConstants.BeatElement e = beatConstants.bonusClips[index];
    currentAudioSource.clip = e.clip;
    beatSynchronizer.currentIndex = index;
    beatSynchronizer.bpm = e.bpm;
    beatSynchronizer.startDelay = e.startDelay;
    beatSynchronizer.volumeSmall = e.volumeSmall;
    beatSynchronizer.volumeBig = e.volumeBig;
    beatSynchronizer.enabled = false;
    beatSynchronizer.enabled = true;
    foreach (BeatCounter counter in counters) {
      counter.init();
    }

  }

  public void selectSource(int index) {
    counters = GetComponents<BeatCounter>();
    beatConstants = GetComponent<BeatConstants>();
    currentAudioSource = GetComponent<AudioSource>();
    beatSynchronizer = GetComponent<BeatSynchronizer>();
    BeatConstants.BeatElement e = beatConstants.clips[index];
    currentAudioSource.clip = e.clip;
    beatSynchronizer.currentIndex = index;
    beatSynchronizer.bpm = e.bpm;
    beatSynchronizer.startDelay = e.startDelay;
    beatSynchronizer.volumeSmall = e.volumeSmall;
    beatSynchronizer.volumeBig = e.volumeBig;
    beatSynchronizer.enabled = false;
    beatSynchronizer.enabled = true;
    foreach (BeatCounter counter in counters) {
      counter.init();
    }
    //currentPitchModifier = 0.0f;
  }
  
  public int getLastMusicIndex() {
    return beatConstants.clips.Length - 1;
  }

  public void recoverPitch () {
    moveToPitch(1f + currentPitchModifier, 0f);
  }
  
  public float movePitchToPercent(float percentage, float interval) {
    float currentPitch = 1f + currentPitchModifier;
    float movedAmount = currentPitch * percentage - currentPitch;
    movePitch(movedAmount, interval);
    return movedAmount;
  }
  
  public void movePitch(float value, float interval) {
    currentPitchModifier += value;
    // nextPitch is for calculating bpm of beat counters
    moveToPitch(1f + currentPitchModifier, interval);
  }

  public void moveToPitch(float to, float interval) {
    Debug.Log("Moving pitch to " + to + " taking " + interval + " seconds");
    currentPitchModifier = to - 1f;
    if (!DataManager.dm.isBonusStage)
      beatSynchronizer.bpm = beatConstants.clips[beatSynchronizer.currentIndex].bpm * (to);
    else
      beatSynchronizer.bpm = beatConstants.bonusClips[beatSynchronizer.currentIndex].bpm * (to);

    // Reserve pitch change to the beat counter, so the pitch can be changed on the beat
    if (currentMovePitchCoroutine != null)
      StopCoroutine(currentMovePitchCoroutine);
    if (interval == 0f) {
      currentAudioSource.pitch = to;
    } else {
      currentMovePitchCoroutine = movePitchCoroutine(to - currentAudioSource.pitch, interval);
      StartCoroutine(currentMovePitchCoroutine);
    }
    foreach (BeatCounter counter in counters) {
      counter.modifyBPM(beatSynchronizer.bpm);
    }
  }

  IEnumerator movePitchCoroutine(float value, float interval) {
    float time = 0.0f;
    float remain = value;
    while (time < interval) {
      time += Time.deltaTime;
      float changeAmount = value * (Time.deltaTime) / interval;
      currentAudioSource.pitch += changeAmount;
      remain -= changeAmount;
      yield return null;
    }
    // If the interval is too short and above loop did not run, just change pitch like follows.
    // It also adjusts correct error for the Time.deltaTime.
    currentAudioSource.pitch += remain;
  }
}
