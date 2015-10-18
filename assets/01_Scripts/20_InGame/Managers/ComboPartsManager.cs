using UnityEngine;
using System.Collections;

public class ComboPartsManager : ObjectsManager {
  public PartsToBeCollected ptb;
  public Transform normalParts;

  public int chanceBase = 100;
  public GameObject goldenCubePrefab;
  public GoldCubesCount gcCount;
  public int goldCubesGet = 10;
  public int goldenCubeChance = 1;

  public GameObject superheatPartPrefab;
  public int superheatPartChance = 5;

  public GameObject objPrefab_next;

  public int[] fullComboCountPerLevel;

  public float radius = 20;

  public float pitchStart = 0.4f;
  public float pitchIncrease = 0.05f;

  private bool trying = false;
  private bool secondShot = false;

  public GameObject nextInstance;
  private int comboCount = 0;
  private int fullComboCount;
  private Mesh[] partsMeshes;

  override public void initRest() {
    skipInterval = true;
    partsMeshes = new Mesh[normalParts.childCount];
    int count = 0;
    foreach (Transform tr in normalParts) {
      partsMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }
  }

  override public void adjustForLevel(int level) {
    fullComboCount = fullComboCountPerLevel[level];
  }

  override protected void afterSpawn() {
    instance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
    trying = false;
    comboCount = 0;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 currentV = instance.transform.position;
    Vector3 spawnPosition = new Vector3(currentV.x + randomV.x * radius, 0, currentV.z + randomV.y * radius);

    nextInstance = (GameObject) Instantiate (objPrefab_next, spawnPosition, Quaternion.identity);
    nextInstance.transform.parent = transform;
    nextInstance.GetComponent<OffsetFixer>().setParent(instance);

    int random = Random.Range(0, chanceBase);
    if (random < goldenCubeChance) {
      changeObject(nextInstance, goldenCubePrefab);
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(true);
    } else if (random < superheatPartChance) {
      changeObject(nextInstance, superheatPartPrefab);
    } else {
      nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
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

  public void tryToGet() {
    if (trying) {
      if (secondShot) {
        secondShot = false;
        if (instance != null) instance.GetComponent<ComboPartMover>().destroyObject();
      } else {
        secondShot = true;
      }
    }
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;
    secondShot = false;

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
    instance = (GameObject) Instantiate (objPrefab, spawnPos, spawnRotation);
    instance.transform.parent = transform;
    changeObject(instance, nextInstance);

    if (compareEqualMesh(nextInstance, goldenCubePrefab)) {
      instance.GetComponent<Renderer>().sharedMaterial = goldenCubePrefab.GetComponent<Renderer>().sharedMaterial;
      instance.GetComponent<SphereCollider>().radius = goldenCubePrefab.GetComponent<SphereCollider>().radius;
      instance.GetComponent<ComboPartMover>().setGolden();
      instance.transform.Find("BasicEffect").gameObject.SetActive(false);
      instance.transform.Find("GoldenEffect").gameObject.SetActive(true);
    } else if (compareEqualMesh(nextInstance, superheatPartPrefab)) {
      instance.GetComponent<Renderer>().sharedMaterial = superheatPartPrefab.GetComponent<Renderer>().sharedMaterial;
      instance.GetComponent<SphereCollider>().radius = superheatPartPrefab.GetComponent<SphereCollider>().radius;
      instance.GetComponent<ComboPartMover>().setSuper();
    }

    Destroy(nextInstance);

    if (comboCount + 1 < fullComboCount) {
      Vector2 randomV = Random.insideUnitCircle;
      randomV.Normalize();
      Vector3 nextSpawnPos = new Vector3(spawnPos.x + randomV.x * radius, 0, spawnPos.z + randomV.y * radius);
      nextInstance = (GameObject) Instantiate (objPrefab_next, nextSpawnPos, spawnRotation);
      nextInstance.transform.parent = transform;
      nextInstance.GetComponent<OffsetFixer>().setParent(instance);

      int random = Random.Range(0, 100);
      if (random < goldenCubeChance) {
        changeObject(nextInstance, goldenCubePrefab);
      } else if (random < superheatPartChance) {
        changeObject(nextInstance, superheatPartPrefab);
      } else {
        nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
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
