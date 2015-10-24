using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour {
  public static CubeManager cm;
  public TimeMonsterManager tmm;
  public OffscreenObjectIndicator spaceShipIndicator;
  public Superheat superheat;
  public Transform cuberNeedsMore;
  public PartsCollector cuber;
  public PlayerMover player;
  public GameObject spaceShipDebris;
  private Text currentCubesText;
  private Text requiredCubesText;
  private int currentCubes = 0;
  private float currentCubesCount = 0;
  private int requiredCubes;

  public int increaseSpeed = 2;
  private int totalCount = 0;
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

    currentCubesText = cuberNeedsMore.Find("Current").GetComponent<Text>();
    requiredCubesText = cuberNeedsMore.Find("Required").GetComponent<Text>();
    resetProgress(false);
  }

  public void resetProgress(bool nextPhase = true) {
    if (nextPhase) {
      cuberNeedsMore.gameObject.SetActive(true);
      PhaseManager.pm.nextPhase();
      TimeManager.time.setLimit(false);
      spaceShipIndicator.stopIndicate();
      tmm.stopMonster();
    }
    requiredCubes = 10;
    cuber.setUserFollow(true, requiredCubes);
    currentCubesText.text = "0";
    requiredCubesText.text = "/" + requiredCubes;
  }

  void startFindNextDebris() {
    currentCubes = 0;
    currentCubesCount = 0;

    cuberNeedsMore.gameObject.SetActive(false);

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= 1000;

    Vector3 spawnPos = screenToWorld(screenPos);
    GameObject debris = (GameObject) Instantiate(spaceShipDebris, spawnPos, Quaternion.identity);
    spaceShipIndicator.startIndicate(debris);
    cuber.transform.position = spawnPos;
    cuber.setUserFollow(false);

    TimeManager.time.setLimit(true);
  }

  bool isCuberCharging() {
    return cuberNeedsMore.gameObject.activeSelf;
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
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

    if (isCuberCharging()) {
      currentCubes += cubesGet + bonus;
      currentCubes = currentCubes > requiredCubes ? requiredCubes : currentCubes;
    }

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
    EnergyManager.energy.getHealthByCubes(cubesGet + bonus);
    TimeManager.time.addProgressByCube(cubesGet + bonus);
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

    if (isCuberCharging()) {
      if (currentCubesCount < currentCubes) {
        currentCubesCount = Mathf.MoveTowards(currentCubesCount, currentCubes, Time.deltaTime * (currentCubes - currentCubesCount) * increaseSpeed);
        currentCubesText.text = currentCubesCount.ToString("0");
        if (int.Parse(currentCubesText.text) >= requiredCubes) {
          startFindNextDebris();
        }
      }
    }
  }
}
