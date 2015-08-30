using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesYouHave : MonoBehaviour {
  private Text cubes;

	void OnEnable () {
    cubes = GetComponent<Text>();
    GetComponent<Text>().text = ((int)GameController.control.cubes["now"]).ToString();
	}

  public int youHave() {
    return int.Parse(cubes.text);
  }

  public void buy(int price) {
    cubes.text = (int.Parse(cubes.text) - price).ToString();
    GameController.control.cubes["now"] = int.Parse(cubes.text);
    GameController.control.cubes["used"] = (int)GameController.control.cubes["used"] + price;
  }
}
