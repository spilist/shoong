using UnityEngine;
using System.Collections;

public class CubesCount : MonoBehaviour {
  private int count = 0;
  public ComboBar comboBar;

  public void addCount() {
    count += comboBar.getComboRatio();
  }

  public void addCount(int cubesGet) {
    count += cubesGet;
  }

  public int getCount() {
    return count;
  }
}
