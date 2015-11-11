using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour {
  public static CubeManager cm;
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

  public void showPoints(int amount, Vector3 position) {
    if (amount == 0) return;

    GameObject instance = getPooledObj(pointPool, pointsGet);
    instance.transform.position = position;
    instance.SetActive(true);
    instance.GetComponent<ShowChangeText>().run(amount);
  }

  public void addCount(int amount) {
    EnergyManager.em.getHealthByCubes(amount);

    amount = (int)Mathf.Round(amount * bonusRate);
    totalCount += amount;

    TimeManager.time.addProgressByCube(amount);
  }

  public int getCount() {
    return totalCount + pointsByTime;
  }

  public void moreCubes(int val) {
    bonusRate *= (100 + val) / 100f;
  }

  public void resetCubeAbility() {
    bonusRate = 1;
  }

  void Update() {
    if (gameStarted) {
      if (currentCount < totalCount) {
        currentCount = Mathf.MoveTowards(currentCount, totalCount, Time.deltaTime * (totalCount - currentCount) * increaseSpeed);
      }

      cubesCount.text = (currentCount + pointsByTime).ToString("0");
    }
  }

  public void addPointsByTime() {
    pointsByTime += pointsPerSeconds;
  }

  public void startCount() {
    gameStarted = true;
  }
}
