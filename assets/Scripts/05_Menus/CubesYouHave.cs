using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesYouHave : MonoBehaviour {
  public string which;
  private Text cubes;
  private Hashtable table;

	void OnEnable () {
    cubes = GetComponent<Text>();
    if (which == "golden") {
      table = GameController.control.goldenCubes;
    } else {
      table = GameController.control.cubes;
    }
    cubes.text = ((int)table["now"]).ToString();
	}

  public int youHave() {
    return int.Parse(cubes.text);
  }

  public void buy(int price) {
    cubes.text = (int.Parse(cubes.text) - price).ToString();
    table["now"] = int.Parse(cubes.text);
    table["used"] = (int)table["used"] + price;
  }

  public void add(int amount) {
    cubes.text = (int.Parse(cubes.text) + amount).ToString();
    table["now"] = int.Parse(cubes.text);
    table["total"] = ((int)table["total"]) + amount;
  }
}
