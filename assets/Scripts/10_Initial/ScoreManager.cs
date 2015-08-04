using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
  public PartsCount partsCount;
  public Text partsCount_highScore;
  public ElapsedTime elapsedTime;
  public Text elapsedTime_highScore;

  int partsHighScore = 0;
  string partsHighScoreKey = "PartsHighScore";

  int timeHighScore = 0;
  string timeHighScoreKey = "TimeHighScore";

	void Start () {
    //Get the highScore from player prefs if it is there, 0 otherwise.
    partsHighScore = PlayerPrefs.GetInt(partsHighScoreKey, 0);
    partsCount_highScore.text = partsHighScore.ToString();

    timeHighScore = PlayerPrefs.GetInt(timeHighScoreKey, 0);
    elapsedTime_highScore.text = timeHighScore.ToString();
	}

  void Update() {
    if (partsCount.getCount() > partsHighScore) {
      partsCount_highScore.text = partsCount.getCount().ToString();
    }

    if (elapsedTime.getTime() > timeHighScore) {
      elapsedTime_highScore.text = elapsedTime.getTime().ToString();
    }
  }

  void OnDisable(){
    if (partsCount.getCount() > partsHighScore) {
      PlayerPrefs.SetInt(partsHighScoreKey, partsCount.getCount());
    }

    if (elapsedTime.getTime() > timeHighScore) {
      PlayerPrefs.SetInt(timeHighScoreKey, elapsedTime.getTime());
    }

    PlayerPrefs.Save();
  }
}
