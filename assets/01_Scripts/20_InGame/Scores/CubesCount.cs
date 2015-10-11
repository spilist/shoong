using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesCount : MonoBehaviour {
  public PowerBoost powerBoost;

  public int increaseSpeed = 2;
  private int count = 0;
  private float currentCount = 0;
  private Text countText;
  private int cubesHighscore = 0;
  public Text cubesHighscoreText;
  public GameObject howManyCubesGet;
  public GameObject howManyBonusCubesGet;
  public GameObject cubesGetOnSuperheat;

  void Start() {
    countText = GetComponent<Text>();
    cubesHighscore = DataManager.dm.getInt("BestCubes");
    cubesHighscoreText.text = cubesHighscore.ToString();
  }

  public void addCount(int cubesGet, int bonus = 0) {
    count += cubesGet + bonus;

    if (powerBoost.isOnPowerBoost()) {
      GameObject instance = Instantiate(cubesGetOnSuperheat);
      instance.transform.SetParent(transform.parent.transform, false);
      instance.GetComponent<ShowChangeText>().run(cubesGet);
    } else {
      GameObject cubesGetInstance = Instantiate(howManyCubesGet);
      cubesGetInstance.transform.SetParent(transform.parent.transform, false);
      cubesGetInstance.GetComponent<ShowChangeText>().run(cubesGet);

      if (bonus > 0) {
        GameObject bonusInstance = Instantiate(howManyBonusCubesGet);
        bonusInstance.transform.SetParent(transform.parent.transform, false);
        bonusInstance.GetComponent<ShowChangeText>().run(bonus);
      }
      powerBoost.addGuage((cubesGet + bonus) * powerBoost.guagePerCube);
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
