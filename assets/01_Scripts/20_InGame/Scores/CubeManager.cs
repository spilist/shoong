using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour {
  public static CubeManager cm;
  public Transform inGameUI;
  private int totalCount = 0;
  public int increaseSpeed = 5;
  public Text cubesCount;
  private float currentCount = 0;

  public int pointsPerSeconds = 10;
  private float bonusRate;
  private int pointsByTime;
  private bool gameStarted = false;

  void Awake() {
    cm = this;
    cubesCount.text = "0";
    bonusRate = 1;
  }

  public void addCount(int cubesGet) {
    EnergyManager.em.getHealthByCubes(cubesGet);

    cubesGet = (int)Mathf.Round(cubesGet * bonusRate);
    totalCount += cubesGet;

    TimeManager.time.addProgressByCube(cubesGet);
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
