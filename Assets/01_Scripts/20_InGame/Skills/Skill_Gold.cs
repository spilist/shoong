using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Gold : Skill {
  public Material goldenPartMaterial;
  public float laserShootDuration = 0.1f;
  public GameObject goldenLaser;
  public List<GameObject> goldenLaserPool;
  public GameObject goldenParticle;
  public List<GameObject> goldenParticlePool;
  public int laserAmount = 10;

  override public void afterStart() {
    goldenLaserPool = new List<GameObject>();
    goldenParticlePool = new List<GameObject>();
    for (int i = 0; i < laserAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(goldenLaser);
      obj.SetActive(false);
      goldenLaserPool.Add(obj);

      obj = (GameObject) Instantiate(goldenParticle);
      obj.SetActive(false);
      goldenParticlePool.Add(obj);
    }
  }

  public GameObject getLaser(Vector3 pos) {
    return getPooledObj(goldenLaserPool, goldenLaser, null, pos);
  }

  public void getParticle(Vector3 pos) {
    GameObject obj = getPooledObj(goldenParticlePool, goldenParticle, null, pos);
    obj.SetActive(true);
  }
}
