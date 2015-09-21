using UnityEngine;
using System.Collections;

public class DoppleManager : ObjectsManager {
  public GameObject energyCube;
  public GameObject forceFieldPrefab;
  public float[] forceFieldSizePerLevel;
  public float targetSize;

  public float waveAwakeDuration = 0.3f;

  override public void initRest() {
    int level = DataManager.dm.getInt("DoppleLevel") - 1;

    level = 2;

    targetSize = forceFieldSizePerLevel[level];

    skipInterval = true;
  }

  override protected void afterSpawn() {
    instance.GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
  }

}
