using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {
  public GameObject monster;
  public float speed_chase = 80;
  public float speed_runaway = 120;
  public float tumble = 3f;
  public float minSpawnInterval = 5f;
  public float maxSpawnInterval = 10f;
  public float spawnRadius = 600;

  private Transform playerTransform;

	void Start () {
    playerTransform = GameObject.Find("Player").transform;
	}

  public void run() {
    StartCoroutine("spawnMonster");
  }

	IEnumerator spawnMonster() {
    float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
    yield return new WaitForSeconds(interval);

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + playerTransform.position.x, playerTransform.position.y, screenPos.y + playerTransform.position.z);

    GameObject newInstance = (GameObject) Instantiate(monster, spawnPos, Quaternion.identity);
    newInstance.transform.parent = gameObject.transform;
  }
}
