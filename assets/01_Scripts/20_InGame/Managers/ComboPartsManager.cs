using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboPartsManager : ObjectsManager {
  public PartsToBeCollected ptb;

  public float illusionLifeTime = 3;
  public float blinkBeforeDestroy = 1.2f;

  public float showDurationStart = 0.35f;
  public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.15f;
  public float emptyDurationDecrease = 0.05f;

  public int chanceBase = 100;
  public GameObject goldenCubePrefab;
  public GoldCubesCount gcCount;
  public GameObject goldCubeDestroyParticle;
  public int goldCubesGet = 10;
  public int goldenCubeChance = 1;

  public GameObject superheatPartPrefab;
  public int superheatPartChance = 5;

  public GameObject objPrefab_next;
  public List<GameObject> objNextPool;

  public int[] fullComboCountPerLevel;

  public float radius = 20;

  public float pitchStart = 0.4f;
  public float pitchIncrease = 0.05f;

  private bool trying = false;

  public GameObject nextInstance;
  public int comboCount = 0;
  public int fullComboCount;
  private Mesh[] partsMeshes;

  override public void initRest() {
    skipInterval = true;

    partsMeshes = new Mesh[GetComponent<NormalPartsManager>().meshes.childCount];
    int count = 0;
    foreach (Transform tr in GetComponent<NormalPartsManager>().meshes) {
      partsMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }

    objNextPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(objPrefab_next);
      obj.SetActive(false);
      obj.transform.parent = transform;
      objNextPool.Add(obj);
    }
  }

  override public void adjustForLevel(int level) {
    fullComboCount = fullComboCountPerLevel[level];
    if (level == 0) {
      goldenCubeChance = 0;
      superheatPartChance = 0;
    }

    if (level == 1) {
      goldenCubeChance = 0;
    }
  }

  override protected void afterSpawn() {
    instance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
    trying = false;
    comboCount = 0;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 currentV = instance.transform.position;
    Vector3 spawnPosition = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);

    nextInstance = getPooledObj(objNextPool, objPrefab_next, spawnPosition);
    nextInstance.SetActive(true);
    nextInstance.GetComponent<OffsetFixer>().setParent(instance);

    int random = Random.Range(0, chanceBase);
    if (random < goldenCubeChance) {
      changeObject(nextInstance, goldenCubePrefab);
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(true);
    } else if (random < superheatPartChance) {
      changeObject(nextInstance, superheatPartPrefab);
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
    } else {
      nextInstance.transform.localScale = objPrefab.transform.localScale;
      nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(true);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
    }
  }

  Mesh getRandomMesh() {
    return partsMeshes[Random.Range(0, partsMeshes.Length)];
  }

  void changeObject(GameObject obj, GameObject changeTo) {
    obj.GetComponent<MeshFilter>().sharedMesh = changeTo.GetComponent<MeshFilter>().sharedMesh;
    obj.transform.localScale = changeTo.transform.localScale;
  }

  bool compareEqualMesh(GameObject obj1, GameObject obj2) {
    return obj1.GetComponent<MeshFilter>().sharedMesh == obj2.GetComponent<MeshFilter>().sharedMesh;
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;

    if (comboCount == 1) {
      player.encounterObject("ComboPart");
    }

    if (comboCount == fullComboCount) {
      DataManager.dm.increment("NumCompleteIllusion");
      player.showEffect("Great", DataManager.dm.getInt("ComboPartsLevel"));
      run();
      return;
    }

    Vector3 spawnPos = nextInstance.transform.position;
    Quaternion spawnRotation = nextInstance.transform.rotation;
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.transform.rotation = spawnRotation;
    instance.SetActive(true);
    changeObject(instance, nextInstance);
    instance.GetComponent<ComboPartMover>().setDestroyAfter();

    if (compareEqualMesh(nextInstance, goldenCubePrefab)) {
      instance.GetComponent<ComboPartMover>().setGolden();
    } else if (compareEqualMesh(nextInstance, superheatPartPrefab)) {
      instance.GetComponent<ComboPartMover>().setSuper();
    } else {
      instance.GetComponent<ComboPartMover>().setNormal();
    }

    nextInstance.SetActive(false);
    nextInstance = null;

    if (comboCount + 1 < fullComboCount) {
      Vector2 randomV = Random.insideUnitCircle;
      randomV.Normalize();
      Vector3 nextSpawnPos = new Vector3(spawnPos.x + randomV.x * radius, 0, spawnPos.z + randomV.y * radius);
      nextInstance = getPooledObj(objNextPool, objPrefab_next, nextSpawnPos);
      nextInstance.SetActive(true);
      nextInstance.transform.rotation = spawnRotation;
      nextInstance.GetComponent<OffsetFixer>().setParent(instance);

      int random = Random.Range(0, 100);
      if (random < goldenCubeChance) {
        changeObject(nextInstance, goldenCubePrefab);
        nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
        nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(true);
      } else if (random < superheatPartChance) {
        changeObject(nextInstance, superheatPartPrefab);
        nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
        nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
      } else {
        nextInstance.transform.localScale = objPrefab.transform.localScale;
        nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
        nextInstance.transform.Find("BasicEffect").gameObject.SetActive(true);
        nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
      }
    }
  }

  public int getComboCount() {
    return comboCount;
  }

  override public int cubesWhenEncounter() {
    return (comboCount + 1) * cubesByEncounter;
  }

  override protected float spawnInterval() {
    if (!trying) return 0;
    else return Random.Range(minSpawnInterval, maxSpawnInterval);
  }
}
