using UnityEngine;
using System.Collections;

public class ObjectsManager : MonoBehaviour {
  public bool forceSpawnAtStart = false;
  public GameObject objPrefab;
  public GameObject objDestroyEffect;
  public GameObject objEncounterEffect;
  public ParticleSystem objEncounterEffectForPlayer;
  public GameObject strengthenPlayerEffect;

  public float strength = 0;
  public float speed = 0;
  public float tumble = 1;
  public float minSpawnInterval = 8;
  public float maxSpawnInterval = 12;
  public int cubesByEncounter;
  public bool isNegative = false;
  public bool hasEncounterEffect = true;

  protected SpawnManager spawnManager;
  public SuperheatPartManager shm;
  public PlayerMover player;
  protected bool skipInterval = false;

  public GameObject instance;
  public int gaugeWhenDestroy = 0;
  public bool hasLevel = false;
  public string nameForLevel;
  public int level;
  protected bool noMoreRespawn = false;

  void OnEnable() {
    spawnManager = gameObject.GetComponent<SpawnManager>();
    shm = gameObject.GetComponent<SuperheatPartManager>();
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    if (forceSpawnAtStart) {
      minSpawnInterval = 0;
      maxSpawnInterval = 0;
    }

    if (hasLevel) {
      level = DataManager.dm.getInt(nameForLevel + "Level") - 1;
      if (level < 0) level = 0;
      adjustForLevel(level);
    }
    initRest();
  }

  virtual public void initRest() {}

  virtual public void run() {
    if (strengthenPlayerEffect != null) {
      strengthenPlayerEffect.SetActive(false);
    }
    StartCoroutine(respawnRoutine());
  }

  public void runByTransform(Vector3 pos, int level) {
    noMoreRespawn = true;
    if (instance != null) {
      instance.GetComponent<ObjectsMover>().destroyObject(false, false);
    }
    instance = (GameObject) Instantiate (objPrefab, pos, Quaternion.identity);
    instance.transform.parent = transform;
    adjustForLevel(level);
    afterSpawn();
  }

  virtual public void runImmediately() {
    skipInterval = true;
    StartCoroutine(respawnRoutine());
  }

  virtual public void adjustForLevel(int level) {}

  protected IEnumerator respawnRoutine() {
    if (noMoreRespawn) yield break;

    yield return new WaitForSeconds(spawnInterval());

    skipInterval = false;

    spawn();
    afterSpawn();
  }

  virtual protected float spawnInterval() {
    if (skipInterval) return 0;
    else return Random.Range(minSpawnInterval, maxSpawnInterval);
  }

  virtual protected void spawn() {
    instance = spawnManager.spawn(objPrefab);
  }

  virtual protected void afterSpawn() {}

  virtual public float getSpeed() {
    return speed;
  }

  virtual public float getTumble() {
    return tumble;
  }

  virtual public Vector3 getDirection() {
    Vector2 randomV = Random.insideUnitCircle;
    return new Vector3(randomV.x, 0, randomV.y).normalized;
  }

  public void skipRespawnInterval() {
    skipInterval = true;
  }

  virtual public int cubesWhenEncounter() {
    return cubesByEncounter;
  }

}
