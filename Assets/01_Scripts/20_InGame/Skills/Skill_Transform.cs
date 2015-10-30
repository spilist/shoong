using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Transform : Skill {
  public int goldRatio = 30;
  public float laserShootDuration = 0.1f;
  public float transformDuration = 0.2f;
  public GameObject transformLaser;
  public List<GameObject> laserPool;
  public GameObject transformParticle;
  public List<GameObject> particlePool;
  public int laserAmount = 10;

  override public void afterStart() {
    laserPool = new List<GameObject>();
    particlePool = new List<GameObject>();
    for (int i = 0; i < laserAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(transformLaser);
      obj.SetActive(false);
      laserPool.Add(obj);

      obj = (GameObject) Instantiate(transformParticle);
      obj.SetActive(false);
      particlePool.Add(obj);
    }
  }

  public GameObject getLaser(Vector3 pos) {
    return getPooledObj(laserPool, transformLaser, pos);
  }

  public GameObject getParticle(Vector3 pos) {
    return getPooledObj(particlePool, transformParticle, pos);
  }
}
