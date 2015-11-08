using UnityEngine;
using System.Collections;

public class NormalPartsManager : ObjectsManager {
  public Transform meshes;
  public PartsToBeCollected ptb;
  public float minUnstableSpeed = 5;
  public float maxUnstableSpeed = 20;

  public GameObject[] partsPrefab;
  private bool unstable = false;

  override public void initRest() {
    spawnPooledObjs(objPool, objPrefab, objAmount);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void startPhase() {
    unstable = true;
  }

  override public float getSpeed() {
    if (unstable) return Random.Range(minUnstableSpeed, maxUnstableSpeed);
    else return speed;
  }

  // public Mesh getRandomMesh() {
  //   return meshes.GetChild(Random.Range(0, meshes.childCount)).GetComponent<MeshFilter>().sharedMesh;
  // }

  public void spawnNormal(Vector3 pos) {
    GameObject obj = getPooledObj(objPool, objPrefab, pos);
    obj.SetActive(true);
  }
}
