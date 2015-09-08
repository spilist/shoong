using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesYouHave : MonoBehaviour {
  public string which;
  private Text cubes;
  private Hashtable table;

	void OnEnable () {
    cubes = GetComponent<Text>();
    cubes.text = DataManager.dm.getInt(which + "Now").ToString();
	}

  public int youHave() {
    return int.Parse(cubes.text);
  }

  public void buy(int price) {
    cubes.text = (int.Parse(cubes.text) - price).ToString();
    DataManager.dm.setInt(which + "Now", int.Parse(cubes.text));
    DataManager.dm.increment(which + "Used", price);
  }

  public void add(int amount) {
    cubes.text = (int.Parse(cubes.text) + amount).ToString();

    DataManager.dm.setInt(which + "Now", int.Parse(cubes.text));
    DataManager.dm.increment(which + "Total", amount);
  }
}
