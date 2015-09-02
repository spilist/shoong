using UnityEngine;
using System.Collections;

public class CubesCount : MonoBehaviour {
  private int count = 0;
  public ComboBar comboBar;
  public GameObject howManyCubesGet;

  public void addCount() {
    addCount(comboBar.getComboRatio());
  }

  public void addCount(int cubesGet) {
    count += cubesGet;

    GameObject cubesGetInstance = Instantiate(howManyCubesGet);
    cubesGetInstance.transform.SetParent(comboBar.transform, false);
    cubesGetInstance.GetComponent<ShowChangeText>().run(cubesGet);
  }

  public int getCount() {
    return count;
  }
}
