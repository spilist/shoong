using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterManager : ObjectsManager {
  public GameObject monster;
  public GameObject hiddenMonster;
  public int cubesWhenDestroy = 100;
  public float speed_chase = 80;
  public float speed_runaway = 120;
  public float speed_weaken = 30;
  public float tumble = 3f;
  public float minSpawnInterval = 5f;
  public float maxSpawnInterval = 10f;
  public float spawnRadius = 600;
  public float detectDistance = 160;
  public float minLifeTime = 10;
  public float maxLifeTime = 15;
  public float weakenDuration = 5.5f;
  public float shrinkUntil = 1.2f;
  public float shrinkSpeed = 2;

  public GameObject minimonPrefab;
  public int numMinimonRespawn = 4;
  public int minimonAdditionalSpeed = 20;
  public float minimonStartTimeByMonster = 0.5f;
  public float minimonStartTimeByPlayer = 1;
  public float minimonStartSpeedByMonster = 40;
  public float minimonStartSpeedByPlayer = 80;
  public float minimonTumble = 10;
  public float minimonLifeTime = 4;
  public int cubesWhenDestroyMinimon = 5;
  public int[] numsMinimonSpawn;
  public int numMinimonSpawn;

  public int maxEnlargeCount = 50;
  public float enlargeScalePerMinimon = 0.01f;
  public int enlargeSpeedPerMinimon = 2;

  public ParticleSystem minimonDestroyEffect;

  public Color weakenedOutlineColor;
  public GameObject monsterFilter;
  public ParticleSystem destroyEffect;
  public OffscreenObjectIndicator indicator;

  public GameObject monsterWarning;
  public float warningBlinkSeconds = 0.7f;

  private Transform playerTransform;

	void Start () {
    playerTransform = GameObject.Find("Player").transform;
    numMinimonSpawn = numsMinimonSpawn[DataManager.dm.getInt("MonsterLevel") - 1];
	}

  override public void run() {
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
