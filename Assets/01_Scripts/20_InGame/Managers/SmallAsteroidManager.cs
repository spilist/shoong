using UnityEngine;
using System.Collections;

public class SmallAsteroidManager : ObjectsManager {
  public int brokenTumble = 30;
  public int minBrokenSpawn = 6;
  public int maxBrokenSpawn = 8;
  public float minBrokenSize = 0.5f;
  public float maxBrokenSize = 1f;
  public int minBrokenSpeed = 200;
  public int maxBrokenSpeed = 400;

  public float minSizeAfterBreak = 5;
  public float maxSizeAfterBreak = 10;
  public float destroyLargeAfter = 0.5f;
  public float destroySmallAfter = 4;
  private bool unstable = false;
  public Transform brokenMeshes;

  override protected void beforeInit() {
    objPrefab.GetComponent<ObjectsMover>().setBoundingSize(objPrefab.GetComponent<Renderer>().bounds.extents.magnitude);
  }

  override public void initRest() {
    spawnPooledObjs(objPool, objPrefab, objAmount);
  }

  override public void run() {}

  override public void runImmediately() {}

  public void startPhase() {
    unstable = true;
  }

  override public float getSpeed() {
    if (unstable) return speed * 2;
    else return speed;
  }

  override public float getTumble() {
    if (unstable) return tumble * 2;
    else return tumble;
  }

  public Mesh getRandomMesh() {
    return brokenMeshes.GetChild(Random.Range(0, brokenMeshes.childCount)).GetComponent<MeshFilter>().sharedMesh;
  }
}
