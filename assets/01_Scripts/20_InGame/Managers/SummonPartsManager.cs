using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SummonPartsManager : ObjectsManager {
  public PartsToBeCollected ptb;
  public GameObject summonedPartPrefab;
  public List<GameObject> summonedPartPool;
  public GameObject summonedPartsDestroyEffect;
  public List<GameObject> summonedPartDestroyPool;
  public int summonPoolAmount = 20;

  public int[] numSpawnZPerLevel;
  public float[] summonedPartLifetimePerLevel;

  public int startSpawnRadius = 50;
  public int numSpawnX = 4;
  public float distanceBtwX = 30;
  public int numSpawnZ = 6;
  public float distanceBtwZ = 30;

  public float summonedPartLifetime = 3;
  public float blinkBeforeDestroy = 1.2f;

  public float showDurationStart = 0.35f;
  public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.15f;
  public float emptyDurationDecrease = 0.05f;

  public int chanceBase = 200;
  public GameObject goldenCubePrefab;
  public GoldCubesCount gcCount;
  public GameObject goldCubeDestroyParticle;
  public int goldCubesGet = 10;
  public int goldenCubeChance = 1;

  public GameObject superheatPartPrefab;
  public int superheatPartChance = 10;

  private Mesh[] summonMeshes;
  private Mesh summonMesh;
  private int getCount = 0;
  private GameObject parentObj;

  override public void initRest() {
    skipInterval = true;

    summonMeshes = new Mesh[GetComponent<NormalPartsManager>().meshes.childCount];
    int count = 0;
    foreach (Transform tr in GetComponent<NormalPartsManager>().meshes) {
      summonMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }

    summonedPartPool = new List<GameObject>();
    summonedPartDestroyPool = new List<GameObject>();
    for (int i = 0; i < summonPoolAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(summonedPartPrefab);
      obj.SetActive(false);
      summonedPartPool.Add(obj);

      obj = (GameObject) Instantiate(summonedPartsDestroyEffect);
      obj.SetActive(false);
      summonedPartDestroyPool.Add(obj);
    }

    parentObj = new GameObject();
    parentObj.transform.SetParent(transform);
  }

  override public void adjustForLevel(int level) {
    numSpawnZ = numSpawnZPerLevel[level];
    summonedPartLifetime = summonedPartLifetimePerLevel[level];
    if (level == 0) {
      goldenCubeChance = 0;
      superheatPartChance = 0;
    }

    if (level == 1) {
      goldenCubeChance = 0;
    }
  }

  Mesh randomMesh() {
    return summonMeshes[Random.Range(0, summonMeshes.Length)];
  }

  public void startSummon() {
    getCount = 0;

    int angle = Random.Range(0, 360);
    Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    Vector3 playerPos = player.transform.position;
    Vector3 origin = new Vector3(playerPos.x + dir.x * startSpawnRadius, 0, playerPos.z + dir.z * startSpawnRadius);

    parentObj.transform.position = origin;
    parentObj.transform.localEulerAngles = new Vector3 (0, angle, 0);

    for (int i = 0; i < numSpawnX; i++) {
      for (int j = 0; j < numSpawnZ; j++) {
        Vector3 spawnPos = new Vector3(distanceBtwX * i - distanceBtwX * (numSpawnX - 1) / 2, 0, distanceBtwZ * j);

        GameObject instance = getPooledObj(summonedPartPool, summonedPartPrefab);
        instance.SetActive(true);
        instance.transform.SetParent(parentObj.transform, false);
        instance.transform.localPosition = spawnPos;

        int random = Random.Range(0, chanceBase);
        if (random < goldenCubeChance) {
          changeObject(instance, goldenCubePrefab);
          instance.transform.Find("GoldenEffect").gameObject.SetActive(true);
          instance.transform.Find("HeatEffect").gameObject.SetActive(false);
          instance.transform.Find("BasicEffect").gameObject.SetActive(false);
        } else if (random < superheatPartChance) {
          changeObject(instance, superheatPartPrefab);
          instance.transform.Find("GoldenEffect").gameObject.SetActive(false);
          instance.transform.Find("HeatEffect").gameObject.SetActive(true);
          instance.transform.Find("BasicEffect").gameObject.SetActive(false);
        } else {
          instance.transform.localScale = summonedPartPrefab.transform.localScale;
          instance.GetComponent<MeshFilter>().sharedMesh = randomMesh();
          instance.transform.Find("GoldenEffect").gameObject.SetActive(false);
          instance.transform.Find("HeatEffect").gameObject.SetActive(false);
          instance.transform.Find("BasicEffect").gameObject.SetActive(true);
        }
      }
    }

    run();
  }

  void changeObject(GameObject obj, GameObject changeTo) {
    obj.GetComponent<MeshFilter>().sharedMesh = changeTo.GetComponent<MeshFilter>().sharedMesh;
    obj.transform.localScale = changeTo.transform.localScale;
    obj.GetComponent<Renderer>().sharedMaterial = changeTo.GetComponent<Renderer>().sharedMaterial;
    obj.GetComponent<SphereCollider>().radius = changeTo.GetComponent<SphereCollider>().radius;

    if (changeTo == goldenCubePrefab) obj.GetComponent<SummonedPartMover>().setGolden();
    else if (changeTo == superheatPartPrefab) obj.GetComponent<SummonedPartMover>().setSuper();
    else obj.GetComponent<SummonedPartMover>().setNormal();
  }

  public void increaseSummonedPartGetcount() {
    getCount++;

    DataManager.dm.increment("NumSummonedPartsGet");

    if (getCount == numSpawnX * numSpawnZ) {
      player.showEffect("Great", DataManager.dm.getInt("SummonPartsLevel"));
      DataManager.dm.increment("NumCompleteSummon");
    }
  }
}
