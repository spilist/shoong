﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesYouHave : MonoBehaviour {
  public string which;
  private Text cubes;
  private Hashtable table;

	void OnEnable () {
    cubes = GetComponent<Text>();
    cubes.text = DataManager.dm.getInt("Current" + which).ToString();
	}

  public int youHave() {
    return int.Parse(cubes.text);
  }

  public void buy(float price) {
    cubes.text = (int.Parse(cubes.text) - (int)price).ToString();
    DataManager.dm.increment("Current" + which, -(int)price);
  }

  public void add(int amount) {
    cubes.text = (int.Parse(cubes.text) + amount).ToString();

    DataManager.dm.increment("Current" + which, amount);
    DataManager.dm.increment("Total" + which, amount);
  }
}
