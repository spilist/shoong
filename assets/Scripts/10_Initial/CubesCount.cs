using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesCount : MonoBehaviour {
  private int count = 0;
  public ComboBar comboBar;
  public GameObject howManyPartsGet;
  public PartsCollector partsCollector;

  public void addCount() {
    addCount(comboBar.getComboRatio());
  }

  public void addCount(int cubesGet) {
    count += cubesGet;
    GetComponent<Text>().text = count.ToString();
    GameObject partsGetInstance = Instantiate(howManyPartsGet);
    partsGetInstance.transform.SetParent(transform.parent.transform, false);
    partsGetInstance.GetComponent<HowManyPartsGet>().run(cubesGet);
    partsCollector.increaseSize(cubesGet);
  }

  public int getCount() {
    return count;
  }
}
