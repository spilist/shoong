using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
  public PartsCount partsCount;
  public Text partsCount_highScore;
  public ElapsedTime elapsedTime;
  public Text elapsedTime_highScore;

  int cubesHighscore = 0;
  int timeHighscore = 0;

	void Start () {
    cubesHighscore = (int) GameController.control.cubes["highscore"];
    partsCount_highScore.text = cubesHighscore.ToString();

    timeHighscore = (int) GameController.control.times["highscore"];
    elapsedTime_highScore.text = timeHighscore.ToString();
	}

  void Update() {
    if (partsCount.getCount() > cubesHighscore) {
      partsCount_highScore.text = partsCount.getCount().ToString();
    }

    if (elapsedTime.getTime() > timeHighscore) {
      elapsedTime_highScore.text = elapsedTime.getTime().ToString();
    }
  }

  public void run() {
    GameController.control.cubes["now"] = (int) GameController.control.cubes["now"] + partsCount.getCount();
    GameController.control.cubes["total"] = (int) GameController.control.cubes["total"] + partsCount.getCount();;
    if (partsCount.getCount() > cubesHighscore) {
      GameController.control.cubes["highscore"] = partsCount.getCount();
    }

    GameController.control.times["total"] = (int) GameController.control.times["total"] + elapsedTime.getTime();
    if (elapsedTime.getTime() > timeHighscore) {
      GameController.control.times["highscore"] = elapsedTime.getTime();
    }

    GameController.control.save();
  }
}
