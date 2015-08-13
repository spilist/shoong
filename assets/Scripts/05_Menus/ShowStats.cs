using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowStats : MonoBehaviour {
  public Text cubes;
  public Text times;

	void OnEnable() {
    cubes.text = "cubes";
    addTextToCubes("now");
    addTextToCubes("used");
    addTextToCubes("total");
    addTextToCubes("highscore");

    times.text = "times";
    addTextToTimes("total");
    addTextToTimes("highscore");
  }

  void addTextToCubes(string str) {
    cubes.text += "\n - " + str + ": " + GameController.control.cubes[str];
  }

  void addTextToTimes(string str) {
    times.text += "\n - " + str + ": " + GameController.control.times[str];
  }
}
