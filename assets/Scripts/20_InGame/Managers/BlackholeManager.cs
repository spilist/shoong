using UnityEngine;
using System.Collections;

public class BlackholeManager : ObjectsManager {
  public GameObject blackhole_prefab;
  public GameObject blackholeGravitySphere_prefab;
  private GameObject blackhole;
  private GameObject blackholeGravity;

  public float minSpawnInterval = 5f;
  public float maxSpawnInterval = 10f;
  public float spawnRadius = 600;

  public int gravity = 50;
  public int pullUser = 50;

  public int reboundDuring = 2;

  override public void run() {
    StartCoroutine("spawnBlackhole");
  }

  IEnumerator spawnBlackhole() {
    if (!skipInterval) {
      float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
      yield return new WaitForSeconds(interval);
    }

    skipInterval = false;

    Vector3 spawnPos = spawnManager.getSpawnPosition(blackhole_prefab);
    blackhole = (GameObject) Instantiate(blackhole_prefab, spawnPos, Quaternion.identity);
    blackhole.transform.parent = transform;

    blackholeGravity = (GameObject) Instantiate(blackholeGravitySphere_prefab, spawnPos, Quaternion.Euler(90, 0, 0));
    blackholeGravity.transform.parent = transform;
    blackholeGravity.GetComponent<BlackholeGravitySphere>().setBlackhole(blackhole);
  }

  override public void skipRespawnInterval() {
    skipInterval = true;
  }

  public GameObject getBlackhole() {
    return blackhole;
  }

  public GameObject getBlackholeGravity() {
    return blackholeGravity;
  }
}
