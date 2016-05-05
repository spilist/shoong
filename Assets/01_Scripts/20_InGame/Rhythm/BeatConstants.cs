using UnityEngine;
using System.Collections;

public class BeatConstants : MonoBehaviour {
  public BeatElement[] clips;
  public BeatElement[] bonusClips;

  // Use this for initialization
  void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  [System.Serializable]
  public class BeatElement {
    public AudioClip clip;
    public float bpm;    // Tempo in beats per minute of the audio clip.
    public float startDelay; // Number of seconds to delay the start of audio playback.
    public float volumeSmall;
    public float volumeBig;
  }
}
