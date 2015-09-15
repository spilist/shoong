using UnityEngine;
using System.Collections;

public class SummonPartsManager : ObjectsManager {
	public GameObject summonPartPrefab;
  public Transform summonedPartsTransformParent;
  public GameObject summonedParts;
  public GameObject partsDestroyEffect;
  public GameObject summonedPartsDestroyEffect;

  public float tumble = 5;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;

  public int startSpawnRadius = 50;
  public int numSpawnX = 4;
  public float distanceBtwX = 30;
  public int numSpawnZ = 6;
  public float distanceBtwZ = 30;

  public float summonedPartLifetime = 3;
  public float blinkAfter = 1.8f;
  public float blinkColorAlpha = 0.5f;
  public Color blinkOutlineColor;

  public float showDurationStart = 0.35f;
  public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.15f;
  public float emptyDurationDecrease = 0.05f;

  private SummonPartMover summonPart;
  private int getCount = 0;

  override public void initRest() {
    skipInterval = true;
  }

  override public float getTumble(string objTag) {
    return tumble;
  }

  override public void run() {
    StartCoroutine(respawn());
  }

  IEnumerator respawn() {
    if (!skipInterval) {
      yield return new WaitForSeconds(summonedPartLifetime + Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    skipInterval = false;
    summonPart = spawnManager.spawn(summonPartPrefab).GetComponent<SummonPartMover>();
  }

  public void startSummon() {
    getCount = 0;

    int angle = Random.Range(0, 360);
    Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    Vector3 playerPos = player.transform.position;
    Vector3 origin = new Vector3(playerPos.x + dir.x * startSpawnRadius, 0, playerPos.z + dir.z * startSpawnRadius);

    summonedPartsTransformParent.position = origin;
    summonedPartsTransformParent.localEulerAngles = new Vector3 (0, angle, 0);

    GameObject partToSummon = summonedParts.transform.GetChild(Random.Range(0, summonedParts.transform.childCount)).gameObject;
    for (int i = 0; i < numSpawnX; i++) {
      for (int j = 0; j < numSpawnZ; j++) {
        Vector3 spawnPos = new Vector3(distanceBtwX * i - distanceBtwX * (numSpawnX - 1) / 2, 0, distanceBtwZ * j);
        GameObject instance = (GameObject) Instantiate(partToSummon, spawnPos, Quaternion.identity);
        instance.transform.SetParent(summonedPartsTransformParent, false);
      }
    }

    run();
  }

  public void increaseSummonedPartGetcount() {
    getCount++;

    if (getCount == numSpawnX * numSpawnZ) {
      // quest
      player.showEffect("Great");
    }
  }
}
