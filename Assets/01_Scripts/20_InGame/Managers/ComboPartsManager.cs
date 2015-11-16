using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboPartsManager : ObjectsManager {
  public float illusionLifeTime = 3;
  public float blinkBeforeDestroy = 1.2f;

  public float showDurationStart = 0.35f;
  public float showDurationDecrease = 0.1f;
  public float emptyDurationStart = 0.15f;
  public float emptyDurationDecrease = 0.05f;

  public int chanceBase = 100;
  public int goldenCubeChance = 1;
  private bool golden = false;

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
  private GoldenCubeManager gcm;

  override public void initRest() {
    skipInterval = true;

    gcm = GetComponent<GoldenCubeManager>();

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

    run();
  }

  override public void adjustForLevel(int level) {
    fullComboCount = fullComboCountPerLevel[level - 1];
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
      nextInstance.GetComponent<MeshFilter>().sharedMesh = gcm.objPrefab.GetComponent<MeshFilter>().sharedMesh;
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(true);
      golden = true;
    } else {
      nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
      nextInstance.transform.Find("BasicEffect").gameObject.SetActive(true);
      nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
      golden = false;
    }
  }

  Mesh getRandomMesh() {
    return partsMeshes[Random.Range(0, partsMeshes.Length)];
  }

  public void eatenByPlayer() {
    comboCount++;
    trying = true;

    if (comboCount == 1) {
      player.encounterObject("ComboPart");
    }

    if (comboCount == fullComboCount) {
      DataManager.dm.increment("NumCompleteIllusion");
      run();
      return;
    }

    if (golden) {
      gcm.spawnGoldenCube(nextInstance.transform.position);
      nextInstance.SetActive(false);
      nextInstance = null;
    } else {
      Vector3 spawnPos = nextInstance.transform.position;
      Quaternion spawnRotation = nextInstance.transform.rotation;
      instance = getPooledObj(objPool, objPrefab, spawnPos);
      instance.transform.rotation = spawnRotation;
      instance.GetComponent<ComboPartMover>().setDestroyAfter();
      instance.GetComponent<MeshFilter>().sharedMesh = nextInstance.GetComponent<MeshFilter>().sharedMesh;

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
          golden = true;
          nextInstance.GetComponent<MeshFilter>().sharedMesh = gcm.objPrefab.GetComponent<MeshFilter>().sharedMesh;
          nextInstance.transform.Find("BasicEffect").gameObject.SetActive(false);
          nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(true);
        } else {
          golden = false;
          nextInstance.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
          nextInstance.transform.Find("BasicEffect").gameObject.SetActive(true);
          nextInstance.transform.Find("GoldenEffect").gameObject.SetActive(false);
        }
      }

      instance.SetActive(true);
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
