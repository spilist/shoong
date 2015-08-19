using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldenCubesYouHave : MonoBehaviour {
  private Text cubes;

  void OnEnable () {
    cubes = GetComponent<Text>();
    cubes.text = ((int)GameController.control.goldenCubes["now"]).ToString();
  }

  public int youHave() {
    return int.Parse(cubes.text);
  }

  public void buy(int price) {
    cubes.text = (int.Parse(cubes.text) - price).ToString();
    GameController.control.goldenCubes["now"] = int.Parse(cubes.text);
    GameController.control.goldenCubes["used"] = (int)GameController.control.goldenCubes["used"] + price;
  }
}
