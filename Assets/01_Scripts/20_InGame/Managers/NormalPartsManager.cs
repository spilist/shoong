using UnityEngine;
using System.Collections;

public class NormalPartsManager : ObjectsManager {
  public GoldenCubeManager gcm;
  public Transform meshes;
  public float popStartScale = 0.2f;
  public int popMinDistance = 50;
  public int popMaxDistance = 120;
  public int poppingSpeed = 200;
  public int popCoinRate = 10;

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

  public void popSweets(int num, Vector3 pos) {
    if (num == 0) return;

    for (int i = 0; i < num; i++) {
      if (Random.Range(0, 100) < popCoinRate) {
        gcm.popCoin(pos);
      } else {
        GameObject obj = getPooledObj(objPool, objPrefab, pos);
        obj.GetComponent<Collider>().enabled = false;
        obj.GetComponent<NormalPartsMover>().pop(Random.Range(popMinDistance, popMaxDistance));
      }
    }
  }
}
