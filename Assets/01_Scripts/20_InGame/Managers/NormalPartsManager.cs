using UnityEngine;
using System.Collections;

public class NormalPartsManager : ObjectsManager {
  public GoldenCubeManager gcm;
  public Transform meshes;
  public float popStartScale = 0.2f;
  public int popMinDistance = 50;
  public int popMaxDistance = 120;
  public int popMaxDistanceForBox = 160;
  public int poppingSpeed = 200;
  public int popCoinRate = 10;
  public float bigPartsProbability = 0.1f;

  void Awake() {
    applyWhenGettingObj = (GameObject obj) => {
      NormalPartsMover mover = obj.GetComponent<NormalPartsMover>();
      if (mover == null)
        return;

      if (UnityEngine.Random.Range(0f, 1f) > bigPartsProbability) {
        mover.setSize(1, 1);
      } else {
        mover.setSize(5, 5);
      }
    };
  }

  override public void initRest() {
    spawnPooledObjs(objPool, objPrefab, objAmount);
  }

  override public void run() {}

  override public void runImmediately() {}

  public Mesh getRandomMesh() {
    return meshes.GetChild(Random.Range(0, meshes.childCount)).GetComponent<MeshFilter>().sharedMesh;
  }

  public void spawnNormal(Vector3 pos) {
    GameObject obj = getPooledObj(objPool, objPrefab, pos);
    obj.SetActive(true);
  }

  public void popSweets(int num, Vector3 pos, bool autoEatAfterPopping = false) {
    if (num == 0) return;

    for (int i = 0; i < num; i++) {
      if (DataManager.dm.isBonusStage || Random.Range(0, 100) < popCoinRate) {
        gcm.popCoin(pos, autoEatAfterPopping);
      } else {
        GameObject obj = getPooledObj(objPool, objPrefab, pos);
        obj.GetComponent<NormalPartsMover> ().setSize (1, 1);
        obj.GetComponent<Collider>().enabled = false;
        StartCoroutine(obj.GetComponent<NormalPartsMover>().pop(Random.Range(popMinDistance, popMaxDistance), autoEatAfterPopping));
      }
    }
  }

  public void popSweetsByBox(int num, float after, float interval, Vector3 pos) {
    StartCoroutine(generateSweets(num, after, interval, pos));
  }

  IEnumerator generateSweets(int num, float after, float interval, Vector3 pos) {
    yield return new WaitForSeconds(after);

    for (int i = 0; i < num; i++) {
      if (Random.Range(0, 100) < popCoinRate) {
        gcm.popCoin(pos);
      } else {
        GameObject obj = getPooledObj(objPool, objPrefab, pos);
        obj.GetComponent<Collider>().enabled = false;
        StartCoroutine(obj.GetComponent<NormalPartsMover>().pop(Random.Range(popMinDistance, popMaxDistanceForBox)));
      }
      yield return new WaitForSeconds(interval);
    }
  }
}
