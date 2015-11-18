using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour {
  public static CubeManager cm;
  public AutoBoosterButton abb;
  public float noAutoBonus = 0.1f;
  public Transform worldUI;
  private int totalCount = 0;
  public int increaseSpeed = 5;
  public Text cubesCount;
  private float currentCount = 0;

  public int pointsPerSeconds = 10;
  private float bonusRate;
  private int pointsByTime;
  private bool gameStarted = false;

  public GameObject pointsGet;
  public List<GameObject> pointPool;
  public int pointsGetAmount = 20;
  public Text untilTop;
  public int untilTopShowDiff = 500;
  public Color aboveHighscoreColor;
  private string untilTopSign = "-";
  private int highscore;
  private bool highscoreReached = false;

  void Awake() {
    cm = this;
    cubesCount.text = "0";
    bonusRate = 1;
  }

  void Start() {
    pointPool = new List<GameObject>();
    for (int i = 0; i < pointsGetAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(pointsGet);
      obj.SetActive(false);
      obj.transform.SetParent(worldUI, false);
      pointPool.Add(obj);
    }

    highscore = DataManager.dm.getInt("BestCubes");
  }

  GameObject getPooledObj(List<GameObject> list, GameObject prefab) {
    for (int i = 0; i < list.Count; i++) {
      if (!list[i].activeInHierarchy) {
        return list[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(prefab);
    obj.transform.SetParent(worldUI, false);
    list.Add(obj);
    return obj;
  }

  public void addPoints(int amount, Vector3 position) {
    if (amount == 0) return;

    amount = (int)Mathf.Round(amount * bonusRate);
    totalCount += amount;

    TimeManager.time.addProgressByCube(amount);

    GameObject instance = getPooledObj(pointPool, pointsGet);
    instance.transform.position = position;
    instance.SetActive(true);
    instance.GetComponent<ShowChangeText>().run(amount);
  }

  public int getCount() {
    return totalCount + pointsByTime;
  }

  public int getBonus() {
    if (abb == null || !abb.isOn()) return (int)Mathf.Floor(getCount() * noAutoBonus);
    else return 0;
  }

  public void moreCubes(int val) {
    bonusRate *= (100 + val) / 100f;
  }

  public void resetCubeAbility() {
    bonusRate = 1;
  }

  void Update() {
    if (gameStarted) {
      updateCount();
      checkAboveHighscore();
      showUntilTop();
    }
  }

  void updateCount() {
    if (currentCount <= totalCount) {
      currentCount = Mathf.MoveTowards(currentCount, totalCount, Time.deltaTime * (totalCount - currentCount) * increaseSpeed);
    }

    cubesCount.text = (currentCount + pointsByTime).ToString("0");
  }

  void checkAboveHighscore() {
    if (highscore > 0 && !highscoreReached && (currentCount + pointsByTime >= highscore)) {
      highscoreReached = true;
      untilTopSign = "+";
      untilTop.color = aboveHighscoreColor;
    }
  }

  void showUntilTop() {
    if (highscore > 0 && (getCount() + untilTopShowDiff) > highscore) {
      untilTop.gameObject.SetActive(true);
      untilTop.text = "TOP " + untilTopSign + Mathf.Abs(currentCount + pointsByTime - highscore).ToString("0");
    }
  }

  public void addPointsByTime() {
    pointsByTime += pointsPerSeconds;
  }

  public void startCount() {
    gameStarted = true;
  }
}
