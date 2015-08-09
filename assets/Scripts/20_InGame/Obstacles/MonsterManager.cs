using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterManager : MonoBehaviour {
  public GameObject monster;
  public float speed_chase = 80;
  public float speed_runaway = 120;
  public float speed_weaken = 30;
  public float tumble = 3f;
  public float minSpawnInterval = 5f;
  public float maxSpawnInterval = 10f;
  public float spawnRadius = 600;
  public float minLifeTime = 10;
  public float maxLifeTime = 15;
  public float weakenDuration = 5.5f;
  public float shrinkUntil = 1.2f;
  public float shrinkSpeed = 2;
  public float restoreSpeed = 0;
  public Color weakenedOutlineColor;
  public GameObject monsterFilter;
  public ParticleSystem destroyEffect;

  public GameObject monsterWarning;
  public float warningBlinkSeconds = 0.7f;

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

    StartCoroutine("startWarning");
  }

  IEnumerator startWarning() {
    monsterWarning.GetComponent<AudioSource>().Play();
    while(true) {
      monsterWarning.GetComponent<Text>().enabled = true;

      yield return new WaitForSeconds(warningBlinkSeconds);

      monsterWarning.GetComponent<Text>().enabled = false;

      yield return new WaitForSeconds(1 - warningBlinkSeconds);
    }
  }

  public void stopWarning() {
    monsterWarning.GetComponent<AudioSource>().Stop();
    monsterWarning.GetComponent<Text>().enabled = false;
    StopCoroutine("startWarning");
  }
}
