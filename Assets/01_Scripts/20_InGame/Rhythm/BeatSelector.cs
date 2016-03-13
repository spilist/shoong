using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatSelector : MonoBehaviour {
  BeatCounter[] counters;
  BeatConstants beatConstants;
  public AudioSource currentAudioSource;
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

  public void selectSource(int index) {
    counters = GetComponents<BeatCounter>();
    beatConstants = GetComponent<BeatConstants>();
    currentAudioSource = GetComponent<AudioSource>();
    beatSynchronizer = GetComponent<BeatSynchronizer>();
    BeatConstants.BeatElement e = beatConstants.clips[index];
    Debug.Log("Music index: " + index + ", name: " + e.clip.name);
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
  
  public int getLastMusicIndex() {
    return beatConstants.clips.Length - 1;
  }

  public void setPitchModifier(float value) {
    currentAudioSource.pitch = 1f + value;
    beatSynchronizer.bpm = beatConstants.clips[beatSynchronizer.currentIndex].bpm * currentAudioSource.pitch;
    foreach (BeatCounter counter in counters) {
      counter.modifyBPM(beatSynchronizer.bpm);
    }
  }

  IEnumerator wait_and_go() {
    while (true) {
      foreach (BeatConstants.BeatElement e in beatConstants.clips) {
        
        yield return new WaitForSeconds(3f);

      }
    }
  }
}
