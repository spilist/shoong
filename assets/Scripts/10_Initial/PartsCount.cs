using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsCount : MonoBehaviour {
  private int count = 0;
  public ComboBar comboBar;

	void Start () {
	}

	void Update () {
	}

  public void addCount() {
    count += comboBar.getComboRatio();
    GetComponent<Text>().text = count.ToString();
  }
}
