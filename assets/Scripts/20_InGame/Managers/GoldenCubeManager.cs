using UnityEngine;
using System.Collections;

public class GoldenCubeManager : ObjectsManager {
  public GameObject goldenCubePrefab;
  public GoldCubesCount gcCount;
  public ParticleSystem destroyEffect;
  public ParticleSystem getEffect;

  public float tumble = 5;
  public float speed = 50;

  public int spawnInterval_min = 10;
  public int spawnInterval_max = 100;
  public int respawnAfter = 120;
  public int numGoldenCubesGet = 10;

  public float spawnRadius = 200;
  public float detectDistance = 200;

  public float generateCubePer = 0.3f;
  public GameObject energyCubePrefab;

  private GameObject goldenCube;
  private float spawnInterval;

  override public void initRest() {
    spawnInterval = Random.Range(spawnInterval_min, spawnInterval_max);
  }

  override public void run() {
    StartCoroutine("spawn");
  }

  IEnumerator spawn() {
    yield return new WaitForSeconds(spawnInterval);

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);

    goldenCube = (GameObject) Instantiate(goldenCubePrefab, spawnPos, Quaternion.identity);
    goldenCube.transform.parent = transform;
  }

  override public int cubesWhenEncounter() {
    return numGoldenCubesGet;
  }

  public void respawn() {
    spawnInterval = respawnAfter;
    run();
  }
}
