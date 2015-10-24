using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour {
  public static CubeManager cm;
  public PartsCollector cuber;
  public Transform inGameUI;
  public Superheat superheat;

  private int totalCount = 0;
  public int increaseSpeed = 5;
  public Text cubesCount;
  private float currentCount = 0;
  private int cubesHighscore = 0;
  public Text cubesHighscoreText;
  public GameObject howManyCubesGet;
  public GameObject howManyBonusCubesGet;
  public GameObject cubesGetOnSuperheat;
  public List<GameObject> cubePool;
  public List<GameObject> bonusCubePool;
  public List<GameObject> cubeOnSuperheatPool;
  public int cubeAmount = 20;

  void Awake() {
    cm = this;
    cubesCount.text = "0";
  }

  void Start() {
    cubesHighscore = DataManager.dm.getInt("BestCubes");
    cubesHighscoreText.text = cubesHighscore.ToString();

    cubePool = new List<GameObject>();
    bonusCubePool = new List<GameObject>();
    cubeOnSuperheatPool = new List<GameObject>();
    for (int i = 0; i < cubeAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(howManyCubesGet);
      obj.SetActive(false);
      obj.transform.SetParent(inGameUI, false);
      cubePool.Add(obj);

      obj = (GameObject) Instantiate(howManyBonusCubesGet);
      obj.SetActive(false);
      obj.transform.SetParent(inGameUI, false);
      bonusCubePool.Add(obj);

      obj = (GameObject) Instantiate(cubesGetOnSuperheat);
      obj.SetActive(false);
      obj.transform.SetParent(inGameUI, false);
      cubeOnSuperheatPool.Add(obj);
    }
  }

  GameObject getPooledObj(List<GameObject> list, GameObject prefab) {
    for (int i = 0; i < list.Count; i++) {
      if (!list[i].activeInHierarchy) {
        return list[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(prefab);
    obj.transform.SetParent(transform.parent, false);
    list.Add(obj);
    return obj;
  }

  public void addCount(int cubesGet, int bonus = 0) {
    totalCount += cubesGet + bonus;

    if (superheat.isOnSuperheat()) {
      GameObject instance = getPooledObj(cubeOnSuperheatPool, cubesGetOnSuperheat);
      instance.SetActive(true);
      instance.GetComponent<ShowChangeText>().run(cubesGet);
    } else {
      GameObject instance = getPooledObj(cubePool, howManyCubesGet);
      instance.SetActive(true);
      instance.GetComponent<ShowChangeText>().run(cubesGet);

      if (bonus > 0) {
        GameObject bonusInstance = getPooledObj(bonusCubePool, howManyBonusCubesGet);
        bonusInstance.SetActive(true);
        bonusInstance.GetComponent<ShowChangeText>().run(bonus);
      }
    }

    superheat.addGuage((cubesGet + bonus) * superheat.guagePerCube);
    EnergyManager.em.getHealthByCubes(cubesGet + bonus);
    TimeManager.time.addProgressByCube(cubesGet + bonus);
    cuber.addEmission(cubesGet + bonus);
  }

  public int getCount() {
    return totalCount;
  }

  void Update() {
    if (currentCount < totalCount) {
      currentCount = Mathf.MoveTowards(currentCount, totalCount, Time.deltaTime * (totalCount - currentCount) * increaseSpeed);
      cubesCount.text = currentCount.ToString("0");

      if (currentCount > cubesHighscore) {
        cubesHighscoreText.text = currentCount.ToString("0");
      }
    }
  }
}
