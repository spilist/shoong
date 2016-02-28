using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Transform : Skill {
  public int goldRatio = 30;
  public int subRatio = 10;
  public float laserShootDuration = 0.1f;
  public float transformDuration = 0.2f;
  public GameObject transformLaser;
  public List<GameObject> laserPool;
  public GameObject transformParticle;
  public List<GameObject> particlePool;
  public int laserAmount = 10;

  public List<string> subManagers;

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

    subManagers = new List<string>();
    addManager("ComboParts");
    addManager("RainbowDonuts");
    addManager("SummonParts");
    addManager("EMP");
  }

  public GameObject getLaser(Vector3 pos) {
    return getPooledObj(laserPool, transformLaser, null, pos);
  }

  public GameObject getParticle(Vector3 pos) {
    return getPooledObj(particlePool, transformParticle, null, pos);
  }

  public void addManager(string managerName) {
    subManagers.Add(managerName);
  }

  public string getRandomManagerName() {
    return subManagers[Random.Range(0, subManagers.Count)];
  }
}
