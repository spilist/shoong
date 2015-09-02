using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesCount : MonoBehaviour {
  public int increaseSpeed = 2;
  private int count = 0;
  private float currentCount = 0;
  private Text countText;
  public ComboBar comboBar;
  public GameObject howManyCubesGet;

  void Start() {
    countText = GetComponent<Text>();
  }

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

  void Update() {
    if (currentCount < count) {
      currentCount = Mathf.MoveTowards(currentCount, count, Time.deltaTime * (count - currentCount) * increaseSpeed);
      countText.text = currentCount.ToString("0");
    }
  }
}
