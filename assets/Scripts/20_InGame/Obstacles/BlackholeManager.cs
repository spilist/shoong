using UnityEngine;
using System.Collections;

public class BlackholeManager : MonoBehaviour {
  public FieldObjectsManager fom;

  public GameObject blackhole_prefab;
  public GameObject blackholeGravitySphere_prefab;
  private GameObject blackhole;

  public float minSpawnInterval = 5f;
  public float maxSpawnInterval = 10f;
  public float spawnRadius = 600;
  public float minLifeTime = 10;
  public float maxLifeTime = 15;

  public int gravity = 50;
  public int pullUser = 50;
  public int exitSpeed = 150;

  public int reboundSpeed = 300;
  public int reboundDuring = 2;

  private bool skipInterval = false;

  void Start () {
  }

  public void run() {
    StartCoroutine("spawnBlackhole");
  }

  IEnumerator spawnBlackhole() {
    if (!skipInterval) {
      float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
      yield return new WaitForSeconds(interval);
    }

    skipInterval = false;

    Vector3 spawnPos = fom.getSpawnPosition("Blackhole");
    blackhole = (GameObject) Instantiate(blackhole_prefab, spawnPos, Quaternion.identity);
    blackhole.transform.parent = transform;

    GameObject blackholeGravity = (GameObject) Instantiate(blackholeGravitySphere_prefab, spawnPos, Quaternion.Euler(90, 0, 0));
    blackholeGravity.transform.parent = transform;
    blackholeGravity.GetComponent<BlackholeGravitySphere>().setBlackhole(blackhole);
  }

  public void skipRespawnInterval() {
    skipInterval = true;
  }

  public GameObject getBlackhole() {
    return blackhole;
  }
}
