using UnityEngine;
using System.Collections;

public class SummonPartsManager : ObjectsManager {
  public GameObject summonedPartPrefab;
  public Transform summonedPartsTransformParent;
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

    summonedPartsTransformParent.position = origin;
    summonedPartsTransformParent.localEulerAngles = new Vector3 (0, angle, 0);

    for (int i = 0; i < numSpawnX; i++) {
      for (int j = 0; j < numSpawnZ; j++) {
        Vector3 spawnPos = new Vector3(distanceBtwX * i - distanceBtwX * (numSpawnX - 1) / 2, 0, distanceBtwZ * j);
        GameObject instance = (GameObject) Instantiate(summonedPartPrefab, spawnPos, Quaternion.identity);
        instance.GetComponent<MeshFilter>().sharedMesh = summonMesh;
        instance.transform.SetParent(summonedPartsTransformParent, false);
      }
    }

    run();
  }

  public void increaseSummonedPartGetcount() {
    getCount++;

    QuestManager.qm.addCountToQuest("SummonParts");

    if (getCount == numSpawnX * numSpawnZ) {
      QuestManager.qm.addCountToQuest("CompleteSummonParts");
      player.showEffect("Great");
    }
  }
}
