using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DangerousEMPManager : ObjectsManager {
  public GameObject particleDestroyByPlayer;
  public List<GameObject> particleDestroyByPlayerPool;
  public GameObject explosion;
  public List<GameObject> explosionPool;
  public GameObject biggerExplosion;
  public List<GameObject> biggerExplosionPool;

  public float minScale = 15;
  public float maxScale = 25;
  public float startDuration = 1;
  public float decreaseDurationPerPulse = 0.1f;
  public float empScale = 70;
  public int empRotatingSpeed = 500;
  public float enlargeDuration = 0.1f;
  public float stayDuration = 1;
  public float shrinkDuration = 0.1f;
  private bool larger = false;
  public float enlargeScale = 2;
  public int loseEnergyBigger = 50;
  public float bounceDuringBigger = 0.5f;

  override public void initRest() {
    TimeManager.time.startSpawnDangerousEMP();

    particleDestroyByPlayerPool = new List<GameObject>();
    explosionPool = new List<GameObject>();
    biggerExplosionPool = new List<GameObject>();
    for (int i = 0; i < objAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(particleDestroyByPlayer);
      obj.SetActive(false);
      particleDestroyByPlayerPool.Add(obj);

      obj = (GameObject) Instantiate(explosion);
      obj.SetActive(false);
      explosionPool.Add(obj);

      obj = (GameObject) Instantiate(biggerExplosion);
      obj.SetActive(false);
      biggerExplosionPool.Add(obj);
    }
    respawn();
  }

  override public void run() {}

  override public void runImmediately() {}

  override public void respawn() {
    int count = objAmount - GameObject.FindGameObjectsWithTag("DangerousEMP").Length;
    if (count > 0) {
      for (int i = 0; i < count; i++) {
        GameObject obj = getPooledObj(objPool, objPrefab, spawnManager.getSpawnPosition(objPrefab));
        obj.SetActive(true);
        if (larger) {
          obj.transform.localScale = enlargeScale * Vector3.one;
          obj.transform.Find("DangerousArea").localScale = (empScale / enlargeScale) * Vector3.one;
        } else {
          obj.transform.Find("DangerousArea").localScale = empScale * Vector3.one;
        }
      }
    }
  }

  public void startLarger() {
    if (!larger) {
      larger = true;
      empScale *= enlargeScale;
      foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DangerousEMP")) {
        obj.transform.localScale = enlargeScale * Vector3.one;
        obj.transform.Find("DangerousArea").localScale = (empScale / enlargeScale) * Vector3.one;
      }
    }
  }

  public int loseEnergy() {
    if (larger) return loseEnergyBigger;
    else return loseEnergyWhenEncounter;
  }

  public GameObject getExplosion(Vector3 position) {
    if (larger) return getPooledObj(biggerExplosionPool, biggerExplosion, position);
    else return getPooledObj(explosionPool, explosion, position);
  }
}
