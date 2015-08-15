using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
  public CubesCount cubesCount;
  public Text cubesCount_highScore;
  public ElapsedTime elapsedTime;
  public Text elapsedTime_highScore;

  int cubesHighscore = 0;
  int timeHighscore = 0;

	void Start () {
    cubesHighscore = (int) GameController.control.cubes["highscore"];
    cubesCount_highScore.text = cubesHighscore.ToString();

    timeHighscore = (int) GameController.control.times["highscore"];
    elapsedTime_highScore.text = timeHighscore.ToString();
	}

  void Update() {
    if (cubesCount.getCount() > cubesHighscore) {
      cubesCount_highScore.text = cubesCount.getCount().ToString();
    }

    if (elapsedTime.getTime() > timeHighscore) {
      elapsedTime_highScore.text = elapsedTime.getTime().ToString();
    }
  }

  public void run() {
    GameController.control.cubes["now"] = (int) GameController.control.cubes["now"] + cubesCount.getCount();
    GameController.control.cubes["total"] = (int) GameController.control.cubes["total"] + cubesCount.getCount();;
    if (cubesCount.getCount() > cubesHighscore) {
      GameController.control.cubes["highscore"] = cubesCount.getCount();
    }

    GameController.control.times["total"] = (int) GameController.control.times["total"] + elapsedTime.getTime();
    if (elapsedTime.getTime() > timeHighscore) {
      GameController.control.times["highscore"] = elapsedTime.getTime();
    }

    GameController.control.save();
  }
}
