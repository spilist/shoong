using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsCount : MonoBehaviour {
  private int count = 0;
  public ComboBar comboBar;
  public GameObject howManyPartsGet;

	void Start () {
	}

	void Update () {
	}

  public void addCount() {
    int partsGet = comboBar.getComboRatio();
    count += partsGet;
    GetComponent<Text>().text = count.ToString();
    GameObject partsGetInstance = Instantiate(howManyPartsGet);
    partsGetInstance.transform.SetParent(transform.parent.transform, false);
    partsGetInstance.GetComponent<HowManyPartsGet>().run(partsGet);
  }
}
