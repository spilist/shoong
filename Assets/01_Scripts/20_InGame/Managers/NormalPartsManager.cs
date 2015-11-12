using UnityEngine;
using System.Collections;

public class NormalPartsManager : ObjectsManager {
  public GoldenCubeManager gcm;
  public Transform meshes;

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
}
