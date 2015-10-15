using UnityEngine;
using System.Collections;

public class SummonPartsManager : ObjectsManager {
  public PartsToBeCollected ptb;
  public GameObject summonedPartPrefab;
  public GameObject summonedPartsDestroyEffect;

  public int[] numSpawnZPerLevel;
  public float[] summonedPartLifetimePerLevel;

  public int startSpawnRadius = 50;
  public int numSpawnX = 4;
  public float distanceBtwX = 30;
  public int numSpawnZ = 6;
  public float distanceBtwZ = 30;

  public float summonedPartLifetime = 3;
  public float blinkBeforeDestroy = 1.2f;
  public float blinkColorAlpha = 0.5f;
  public Color blinkOutlineColor;

  public float showDurationStart = 0.35f;
  public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.15f;
  public float emptyDurationDecrease = 0.05f;

  public GameObject goldenCubePrefab;
  public GoldCubesCount gcCount;
  public int goldCubesGet = 10;
  public int goldenCubeChance = 1;

  public GameObject superheatPartPrefab;
  public int superheatPartChance = 10;

  private Mesh[] summonMeshes;
  private Mesh summonMesh;
  private int getCount = 0;

  override public void initRest() {
    skipInterval = true;
    int level = DataManager.dm.getInt("SummonPartsLevel") - 1;
    numSpawnZ = numSpawnZPerLevel[level];
    summonedPartLifetime = summonedPartLifetimePerLevel[level];

    summonMeshes = new Mesh[GetComponent<NormalPartsManager>().objPrefab.transform.childCount];
    int count = 0;
    foreach (Transform tr in GetComponent<NormalPartsManager>().objPrefab.transform) {
      summonMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }
  }

  override protected void afterSpawn() {
    summonMesh = summonMeshes[Random.Range(0, summonMeshes.Length)];
    foreach (Transform tr in instance.transform) {
      tr.GetComponent<MeshFilter>().sharedMesh = summonMesh;
    }
  }

  public void startSummon() {
    getCount = 0;

    int angle = Random.Range(0, 360);
    Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    Vector3 playerPos = player.transform.position;
    Vector3 origin = new Vector3(playerPos.x + dir.x * startSpawnRadius, 0, playerPos.z + dir.z * startSpawnRadius);

    GameObject obj = new GameObject();
    obj.transform.SetParent(transform);
    obj.transform.position = origin;
    obj.transform.localEulerAngles = new Vector3 (0, angle, 0);

    for (int i = 0; i < numSpawnX; i++) {
      for (int j = 0; j < numSpawnZ; j++) {
        Vector3 spawnPos = new Vector3(distanceBtwX * i - distanceBtwX * (numSpawnX - 1) / 2, 0, distanceBtwZ * j);

        GameObject instance = (GameObject) Instantiate(summonedPartPrefab, spawnPos, Quaternion.identity);
        instance.transform.SetParent(obj.transform, false);

        int random = Random.Range(0, 200);
        if (random < goldenCubeChance) {
          changeObject(instance, goldenCubePrefab);
        } else if (random < superheatPartChance) {
          changeObject(instance, superheatPartPrefab);
        } else {
          instance.GetComponent<MeshFilter>().sharedMesh = summonMesh;
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
