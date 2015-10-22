using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubesCount : MonoBehaviour {
  public Superheat superheat;

  public int increaseSpeed = 2;
  private int count = 0;
  private float currentCount = 0;
  private Text countText;
  private int cubesHighscore = 0;
  public Text cubesHighscoreText;
  public GameObject howManyCubesGet;
  public GameObject howManyBonusCubesGet;
  public GameObject cubesGetOnSuperheat;
  public List<GameObject> cubePool;
  public List<GameObject> bonusCubePool;
  public List<GameObject> cubeOnSuperheatPool;
  public int cubeAmount = 20;

  void Start() {
    countText = GetComponent<Text>();
    cubesHighscore = DataManager.dm.getInt("BestCubes");
    cubesHighscoreText.text = cubesHighscore.ToString();

    cubePool = new List<GameObject>();
    bonusCubePool = new List<GameObject>();
    cubeOnSuperheatPool = new List<GameObject>();
    for (int i = 0; i < cubeAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(howManyCubesGet);
      obj.SetActive(false);
      obj.transform.SetParent(transform.parent, false);
      cubePool.Add(obj);

      obj = (GameObject) Instantiate(howManyBonusCubesGet);
      obj.SetActive(false);
      obj.transform.SetParent(transform.parent, false);
      bonusCubePool.Add(obj);

      obj = (GameObject) Instantiate(cubesGetOnSuperheat);
      obj.SetActive(false);
      obj.transform.SetParent(transform.parent, false);
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
    count += cubesGet + bonus;

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
    ElapsedTime.time.addProgressByCube(cubesGet + bonus);
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
