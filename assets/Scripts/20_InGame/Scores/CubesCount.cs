using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesCount : MonoBehaviour {
  public int increaseSpeed = 2;
  private int count = 0;
  private float currentCount = 0;
  private Text countText;
  private int cubesHighscore = 0;
  public Text cubesHighscoreText;
  public ComboBar comboBar;
  public GameObject howManyCubesGet;
  public GameObject howManyBonusCubesGet;

  void Start() {
    countText = GetComponent<Text>();
    cubesHighscore = DataManager.dm.getInt("CubeHighscore");
    cubesHighscoreText.text = cubesHighscore.ToString();
  }

  public void addCount() {
    addCount(comboBar.getComboRatio());
  }

  public void addCount(int cubesGet, int bonus = 0) {
    count += cubesGet + bonus;

    GameObject cubesGetInstance = Instantiate(howManyCubesGet);
    cubesGetInstance.transform.SetParent(comboBar.transform, false);
    cubesGetInstance.GetComponent<ShowChangeText>().run(cubesGet);

    if (bonus > 0) {
      GameObject bonusInstance = Instantiate(howManyBonusCubesGet);
      bonusInstance.transform.SetParent(comboBar.transform, false);
      bonusInstance.GetComponent<ShowChangeText>().run(bonus);
    }
  }

  public int getCount() {
    return count;
  }

  void Update() {
    if (currentCount < count) {
      currentCount = Mathf.MoveTowards(currentCount, count, Time.deltaTime * (count - currentCount) * increaseSpeed);
      countText.text = currentCount.ToString("0");

      if (currentCount > cubesHighscore) {
        cubesHighscoreText.text = currentCount.ToString("0");
      }
    }
  }
}
