using UnityEngine;
using System.Collections;

public class TransformerManager : ObjectsManager {
  public int subRatio = 20;
  public int mainRatio = 5;

  public int areaRadius = 160;
  public float laserShootDuration = 0.05f;
  public float transformDuration = 0.5f;
  public GameObject transformLaser;
  public GameObject transformParticle;

  override public void initRest() {
  }

  override protected void afterSpawn() {
    instance.transform.Find("Area").localScale = (areaRadius / instance.transform.localScale.x) * Vector3.one;
  }
}
