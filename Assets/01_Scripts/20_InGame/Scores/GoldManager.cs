using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GoldManager : MonoBehaviour {
  public static GoldManager gm;
  public GoldenCubeManager gcm;
  public Transform ingameCollider;
  public Transform ingameUI;
  public Text goldText;
  public ParticleSystem getEffect;
  public Material goldenMat;
  public Text effectAmountText;

  public GameObject goldenCubePrefab;
  public List<GameObject> cubePool;
  public int cubeAmount = 50;
  public int goldenCubeStartSpeed = 80;
  public int goldenCubeFollowSpeed = 1000;

  private int count;
  private int earnThisGame = 0;

  void Awake() {
    gm = this;
    cubePool = new List<GameObject>();
    for (int i = 0; i < cubeAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(goldenCubePrefab);
      obj.SetActive(false);
      cubePool.Add(obj);
    }
  }

  public void startGame() {
    float posX = ingameCollider.localPosition.x;
    ingameCollider.localPosition = new Vector3(posX, 0, - ingameUI.localPosition.z / ingameUI.localScale.x);
  }

  void generateGoldCube(Vector3 pos) {
    GameObject gold = getGoldCube();
    gold.SetActive(true);
    gold.transform.position = pos;
  }

  GameObject getGoldCube() {
    for (int i = 0; i < cubePool.Count; i++) {
      if (!cubePool[i].activeInHierarchy) {
        return cubePool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(goldenCubePrefab);
    cubePool.Add(obj);
    return obj;
  }

	void Start () {
    count = DataManager.dm.getInt("CurrentGoldenCubes");
    goldText.text = count.ToString();
  }

  public void addOutsideGame(int amount) {
    count += amount;

    goldText.text = count.ToString();

    DataManager.dm.increment("CurrentGoldenCubes", amount);
  }

  public void add(Vector3 pos, int amount = 1, bool withEffect = true) {

    DataManager.dm.increment("CurrentGoldenCubes", amount);
    DataManager.dm.increment("TotalGoldenCubes", amount);

    for (int i = 0; i < amount; ++i) {
      generateGoldCube(pos);
      GetComponent<AudioSource>().Play();
    }

    earnThisGame += amount;
  }

  public int earned() {
    return earnThisGame;
  }

  public void addCountIngame() {
    count++;
    goldText.text = count.ToString();
  }

  public int getCount() {
    return count;
  }

  public void buy(int price) {
    count -= price;
    goldText.text = count.ToString();
  }
}
