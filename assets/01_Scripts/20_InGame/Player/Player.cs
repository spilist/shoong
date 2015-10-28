using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
  public static Player pl;
  public float sensitivity;
  public bool stopping;
  public float stopSphereRadius = 0.3f;
  private float stickSpeedScale = 1;
  // private bool stopping = false;
  public int stoppingSpeed = 10;
  public float baseSpeed = 80;
  private float originalBaseSpeed;
  private float speedScale;
  private string moreSpeedCondition;

  public Transform effects;
  public SpawnManager spawnManager;
  public GoldCubesCount gcCount;

  public int cubesAsItIsUntill = 20;
  public int restCubesChangePer = 5;
  public int nearAsteroidBonus = 10;
  public float bonusCubeScaleChange = 0.2f;
  public float bonusCubeMaxBase = 100;

  private float speed;
	private float boosterspeed = 0;
  public float tumble = 4;
  private Vector3 direction;

  public MonsterManager monm;
  public RainbowDonutsManager rdm;
  public DoppleManager dpm;
  public BlackholeManager blm;
  public IceDebrisManager icm;
  private float icedDuration;
  private float icedSpeedFactor;

  private bool reboundingByBlackhole = false;
  private bool bouncing = false;
  private bool bouncingByDispenser = false;
  private float bounceDuration = 0;
  public float shakeBase = 10;
  public float shakeDuring = 0.5f;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;
  public float boosterSpeedDecreaseBase = 70;
  public float boosterSpeedDecreasePerTime = 20;
  private float boosterBonus = 1;

	public GameObject particles;
  public List<GameObject> cubePool;
  public int cubeAmount = 50;
  public Material originalCubeMat;
  public Material goldenCubeMat;
  public Color cubeOriginalColor;
  public Color cubeOriginalTrailColor;
  public Color goldenCubeParticleTrailColor;

  public float reboundSpeed = 300;

  private bool unstoppable = false;
  private bool usingEMP = false;
  private bool ridingMonster = false;
  private bool usingJetpack = false;
  private bool usingDopple = false;
  private bool usingMagnet = false;
  private bool usingTransformer = false;
  private bool iced = false;
  public float originalScale;
  private int minimonCounter = 0;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;
  private Rigidbody rb;

  private int nearAsteroidCounter = 0;

  public float afterStrengthenDuration = 1;
  private bool afterStrengthen = false;
  private float afterStrengthenCount = 0;

  private CharacterChangeManager changeManager;
  public Collider contactCollider;
  public GameObject playerDopple;
  private bool trapped = false;

  private bool usingPowerBoost = false;

  private int numBoosters = 0;
  private int numDestroyObstacles = 0;
  private int numUseObjects = 0;
  public PlayerDirectionIndicator dirIndicator;


	void Awake() {
    pl = this;
    originalBaseSpeed = baseSpeed;
  }

  void Start () {
    changeManager = GetComponent<CharacterChangeManager>();
    changeManager.changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    originalScale = transform.localScale.x;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    setDirection(direction);
    speed = baseSpeed;

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;
    rotatePlayerBody(true);

    cubePool = new List<GameObject>();
    for (int i = 0; i < cubeAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(particles);
      obj.SetActive(false);
      cubePool.Add(obj);
    }
	}

  public GameObject generateCube() {
    for (int i = 0; i < cubePool.Count; i++) {
      if (!cubePool[i].activeInHierarchy) {
        return cubePool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(particles);
    cubePool.Add(obj);
    return obj;
  }

	void FixedUpdate () {
    if (stopping) {
      speed = Mathf.MoveTowards(speed, 0, Time.fixedDeltaTime * stoppingSpeed);
    } else if (usingPowerBoost) {
      speed = SuperheatManager.sm.baseSpeed;
    } else if (usingEMP) {
      speed = 0;
    } else if (isRidingRainbowRoad) {
      speed = rdm.ridingSpeed;
    } else if (reboundingByBlackhole) {
      speed = blm.reboundingSpeed;
    } else if (bouncing) {
      speed = reboundSpeed;
    } else if (bouncingByDispenser) {
      speed = reboundSpeed / 2f;
    } else if (ridingMonster) {
      speed = baseSpeed + minimonCounter * monm.enlargeSpeedPerMinimon;
    } else {
      speed = baseSpeed;
    }

    speed += boosterspeed;

    if (iced && !uncontrollable() && !bouncing) {
      speed *= icedSpeedFactor;
    }

    if (boosterspeed > 0) {
      boosterspeed -= speed / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.deltaTime;
    } else if (boosterspeed <= 0){
      boosterspeed = 0;
    }

    if (!isRidingRainbowRoad && (usingDopple || trapped)) {
      rb.velocity = Vector3.zero;
    } else {
      rb.velocity = direction * speed * stickSpeedScale;
    }
	}

  public float getSpeed() {
    return rb.velocity.magnitude;
  }

  public void setPerpDirection(string dir) {
    int sign = (dir == "LRPanel_left") ? 1 : -1;

    Vector3 perp = new Vector3(-direction.z, 0, direction.x) * sign * Time.fixedDeltaTime * sensitivity;
    setDirection((direction + perp).normalized);
  }

	void OnTriggerEnter(Collider other) {
    string tag = other.tag;

    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (mover == null || mover.dangerous()) return;

    if (ridingMonster && tag != "MiniMonster" && tag != "RainbowDonut") {
      generateMinimon(mover);
      return;
    }

    if (tag == "IceDebris" || tag == "PhaseMonster") {
      mover.destroyObject();
      return;
    }

    if (tag == "MiniMonster") {
      if (!absorbMinimon(mover)) return;
    }

    if (tag == "CubeDispenser") {
      if (!unstoppable && !isUsingRainbow()) return;
    }

    goodPartsEncounter(mover, mover.cubesWhenEncounter(), mover.bonusCubes());
	}

  public bool absorbMinimon(ObjectsMover mover) {
    if (!((MiniMonsterMover)mover).isTimeElapsed()) return false;

    if (minimonCounter < monm.maxEnlargeCount) {
      minimonCounter++;
      transform.localScale += monm.enlargeScalePerMinimon * Vector3.one;
    }
    return true;
  }

  public void generateMinimon(ObjectsMover mover) {
    mover.destroyObject(true, true);
    monm.spawnMinimon(transform.position, monm.numMinimonSpawn);

    DataManager.dm.increment("NumSpawnMinimonster", monm.numMinimonSpawn);
  }

  public int numCubesInstantiate(int howMany) {
    int instantiateCount;
    if (howMany < cubesAsItIsUntill) {
      instantiateCount = howMany;
    } else {
      instantiateCount = cubesAsItIsUntill + Mathf.RoundToInt((howMany - cubesAsItIsUntill) / (float) restCubesChangePer);
    }

    return instantiateCount;
  }

  public void goodPartsEncounter(ObjectsMover mover, int howMany, int bonus = 0, bool encounterPlayer = true) {
    if (howMany > 0) {
      for (int e = 0; e < numCubesInstantiate(howMany); e++) {
        GameObject cube = generateCube();
        cube.transform.position = mover.transform.position;
        cube.SetActive(true);
        if (encounterPlayer && mover.tag == "RainbowDonut") {
          if (rdm.isGolden) {
            cube.GetComponent<Renderer>().sharedMaterial = goldenCubeMat;
            cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
          } else {
            cube.GetComponent<Renderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
            cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
          }
          cube.GetComponent<ParticleMover>().setRainbow();
        } else {
          cube.GetComponent<Renderer>().sharedMaterial = originalCubeMat;
          cube.GetComponent<Renderer>().material.SetColor("_TintColor", cubeOriginalColor);
          cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", cubeOriginalTrailColor);
        }
      }

      if (mover.isNegativeObject()) {
        if (isUsingRainbow()) DataManager.dm.increment("NumDestroyObstaclesOnRainbow");
        else if (unstoppable) DataManager.dm.increment("NumDestroyObstaclesWithMetal");

        generateGoldCube(mover);
      }

      if (bonus > 0) {
        GameObject cube = generateCube();
        cube.transform.position = mover.transform.position;
        cube.SetActive(true);
        if (unstoppable && mover.isNegativeObject()) {
          cube.GetComponent<Renderer>().sharedMaterial = changeManager.metalMat;
          DataManager.dm.increment("TotalBonusesWithMetal", bonus);
        } else {
          cube.GetComponent<Renderer>().sharedMaterial = originalCubeMat;
        }

        cube.transform.localScale = (cube.GetComponent<ParticleMover>().startScale + Mathf.Min(bonus, bonusCubeMaxBase)) * bonusCubeScaleChange * Vector3.one;
      }

      if (mover.tag != "GoldenCube") addCubeCount(howMany, bonus);
    }

    if (encounterPlayer) mover.encounterPlayer();
    else mover.destroyObject(true, true);
  }

  public void addCubeCount(int howMany = 1, int bonus = 0) {
    CubeManager.cm.addCount(howMany, bonus);
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring, bool isGolden) {
    for (int e = 0; e < howMany; e++) {
      GameObject cube = generateCube();
      cube.transform.position = tr.position;

      if (isGolden) {
        cube.GetComponent<Renderer>().sharedMaterial = goldenCubeMat;
        cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
      } else {
        cube.GetComponent<Renderer>().sharedMaterial = originalCubeMat;
        cube.GetComponent<Renderer>().material.SetColor("_TintColor", cubeOriginalColor);
        cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", cubeOriginalTrailColor);
      }

      cube.SetActive(true);
    }

    if (!isGolden) addCubeCount(howMany, 0);

    processCollision(collision);
    bouncingByDispenser = true;
    this.bounceDuration = reboundDuring;
  }

  public void bounce(float bounceDuration, Collision collision) {
    if (bounceDuration == 0) return;

    processCollision(collision);
    bouncing = true;
    this.bounceDuration = bounceDuration;
  }

  public void loseEnergy(int amount, string tag) {
    Camera.main.GetComponent<CameraMover>().shake(shakeDuring, shakeBase * amount / 100);
    EnergyManager.em.loseEnergy(amount, tag);
  }

  public void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  public void rotatePlayerBody(bool continuous = false) {
    if (continuous) {
      string[] rots = PlayerPrefs.GetString("CharacterRotation").Split(',');
      transform.rotation = Quaternion.Euler(float.Parse(rots[0]), float.Parse(rots[1]), float.Parse(rots[2]));

      string[] angVals = PlayerPrefs.GetString("CharacterAngVal").Split(',');
      rb.angularVelocity = new Vector3(float.Parse(angVals[0]), float.Parse(angVals[1]), float.Parse(angVals[2]));
    } else {
      rb.angularVelocity = Random.onUnitSphere * tumble;
    }
  }

  public Vector3 getDirection() {
    return direction;
  }

  public void setTrapped(bool val) {
    if (trapped && !val) {
      DataManager.dm.increment("NumExitCubeDispenser");
    }
    trapped = val;
  }

  public void teleport(Vector3 pos) {
    if (changeManager.isTeleporting() || ScoreManager.sm.isGameOver()) return;

    AudioSource.PlayClipAtPoint(dpm.teleportSound, transform.position, dpm.teleportSoundVolume);

    GameObject[] cubeDispensers = GameObject.FindGameObjectsWithTag("CubeDispenser");
    GameObject cubeDispenser = null;
    float distance1 = 0;
    float distance2 = Mathf.Infinity;

    if (cubeDispensers.Length > 0) {
      cubeDispenser = cubeDispensers[0];
      distance1 = (GetComponent<SphereCollider>().radius * transform.localScale.x) + (cubeDispenser.GetComponent<SphereCollider>().radius * cubeDispenser.transform.localScale.x);
      distance2 = Vector3.Distance(cubeDispenser.transform.position, pos);
    }

    if (trapped) {
      if (distance2 > distance1) {
        GameObject instance = (GameObject) Instantiate(playerDopple, pos, transform.rotation);
        instance.GetComponent<PlayerDopple>().run(GetComponent<MeshFilter>().sharedMesh, GetComponent<Renderer>().sharedMaterial);
        return;
      }
    }

    if (cubeDispenser != null && !unstoppable && distance2 <= distance1) {
      pos = cubeDispenser.transform.position;
      trapped = true;
      DataManager.dm.increment("NumTrappedInCubeDispenser");
    }

    changeManager.teleport(pos);
    DataManager.dm.increment("TotalBlinks");
  }

  public void stopMoving() {
    stopping = true;
    stickSpeedScale = 1;
  }

  public void shootBooster(){
    if (stopping) return;

    if (!RhythmManager.rm.isBoosterOK) {
      RhythmManager.rm.currentCircle.SetActive(false);
      return;
    }

    if (usingDopple) {
      teleport(transform.position + direction * dpm.blinkDistance);
      return;
    }

    if (usingJetpack) {
      DataManager.dm.increment("NumBoostersWithJetpack");
    }

    if (!usingPowerBoost) {
      changeManager.booster.Play();
      changeManager.booster.GetComponent<AudioSource>().Play();
    }

    if (boosterspeed < maxBooster()) {
      boosterspeed += boosterSpeedUp() * boosterBonus;
      boosterspeed = boosterspeed > maxBooster() ? maxBooster() : boosterspeed;
    }

    numBoosters++;
    DataManager.dm.increment("TotalBoosters");
    rotatePlayerBody();
  }

  public int getNumBoosters() {
    return numBoosters;
  }

  public bool isOnSuperheat() {
    return usingPowerBoost;
  }

  public void startPowerBoost() {
    usingPowerBoost = true;
    GetComponent<Renderer>().enabled = false;
    GetComponent<Collider>().enabled = false;
    SkillManager.sm.stopSkills();
    changeManager.changeCharacterToOriginal();
    stopOtherEffects();
  }

  public void stopPowerBoost() {
    usingPowerBoost = false;
    GetComponent<Renderer>().enabled = true;
    GetComponent<Collider>().enabled = true;
    rotatePlayerBody();
    afterStrengthenStart();
  }

  public float maxBooster() {
    if (usingPowerBoost) return SuperheatManager.sm.maxBoosterSpeed;
    else return maxBoosterSpeed * boosterBonus;
  }

  float boosterSpeedUp() {
    if (usingPowerBoost) return SuperheatManager.sm.boosterSpeedUpAmount;
    else return boosterSpeedUpAmount;
  }

  public void setRotateByRainbow(bool val) {
    rainbowPosition = rdm.instance.transform.position;
    rb.isKinematic = val;
    isRotatingByRainbow = val;
  }

  public void setDirection(Vector3 dir, float magnitude = 1) {
    if (magnitude < stopSphereRadius) {
      stopping = true;
      stickSpeedScale = 1;
    } else {
      direction = dir;
      stickSpeedScale = magnitude > 1 ? 1 : magnitude;
      stopping = false;
    }
    dirIndicator.setDirection(dir);
  }

  public void nearAsteroid(bool enter = true, int amount = 1) {
    if (enter) nearAsteroidCounter += amount;
    else nearAsteroidCounter -= amount;
  }

  public bool isNearAsteroid() {
    return nearAsteroidCounter > 0;
  }

  public void showEffect(string effectName, int scale = 1) {
    if (usingPowerBoost || ScoreManager.sm.isGameOver()) return;

    if (effectName == "Whew") {
      boosterspeed += 140;
      changeManager.booster.Play();
      afterStrengthenStart();
      DataManager.dm.increment("TotalWhew");
      // audio needed
    } else if (effectName == "Wow") {
      DataManager.dm.increment("TotalWow");
    } else if (effectName == "Great") {
      DataManager.dm.increment("TotalGreat");
    }

    UIEffect effect = effects.Find(effectName).GetComponent<UIEffect>();
    if (effect.gameObject.activeSelf) effect.gameObject.SetActive(false);
    effect.gameObject.SetActive(true);
    effect.addGuage(scale);
  }

  public void effectedBy(string objTag, bool effectOn = true) {
    if (usingPowerBoost) return;

    if (objTag == "Metal") {
      unstoppable = effectOn;
      if (moreSpeedCondition == "Metal") {
        if (effectOn) baseSpeed *= speedScale;
        else baseSpeed /= speedScale;
      }
    } else if (objTag == "Magnet") {
      usingMagnet = effectOn;
    } else if (objTag == "Monster") {
      ridingMonster = effectOn;
      minimonCounter = 0;
    } else if (objTag == "EMP") {
      usingEMP = effectOn;
      rb.isKinematic = effectOn;
    } else if (objTag == "Dopple") {
      usingDopple = effectOn;
      contactCollider.enabled = !effectOn;
    } else if (objTag == "Transformer") {
      usingTransformer = effectOn;
    } else if (objTag == "IceDebris") {
      iced = effectOn;
      icedDuration = icm.speedRestoreDuring;
      icedSpeedFactor = icm.playerSpeedReduceTo;
    }
  }

  public void stopOtherEffects() {
    iced = false;
    icedDuration = 0;
    icedSpeedFactor = 1;
    icm.strengthenPlayerEffect.SetActive(false);

    bounceDuration = 0;
    reboundingByBlackhole = false;
    bouncingByDispenser = false;
    bouncing = false;

    if (isUsingRainbow()) {
      rdm.destroyInstances();
      isRotatingByRainbow = false;
      isRidingRainbowRoad = false;
    }
  }

  public void contactBlackhole(Collision collision) {
    if (isUsingRainbow()) {
      rdm.destroyInstances();
      DataManager.dm.increment("NumReboundByBlackholeOnRainbow");
    }
    Camera.main.GetComponent<CameraMover>().shakeUntilStop(blm.shakeAmount);
    processCollision(collision);
    reboundingByBlackhole = true;
    bounceDuration = blm.reboundDuring;
  }

  public void afterStrengthenStart() {
    if (usingPowerBoost) return;

    afterStrengthen = true;
    afterStrengthenCount = 0;
    changeManager.afterStrengthenEffect.Play();
  }

  public bool isAfterStrengthen() {
    return afterStrengthen;
  }

  bool isBouncing() {
    return bouncing || bouncingByDispenser || reboundingByBlackhole;
  }

  public bool isUsingJetpack() {
    return usingJetpack;
  }

  public bool isUnstoppable() {
    return unstoppable;
  }

  public bool isRidingMonster() {
    return ridingMonster;
  }

  public bool isUsingMagnet() {
    return usingMagnet;
  }

  public bool isRebounding() {
    return reboundingByBlackhole;
  }

  public void setRidingRainbowRoad(bool val) {
    isRidingRainbowRoad = val;
  }

  public bool isUsingRainbow() {
    return isRotatingByRainbow || isRidingRainbowRoad;
  }

  public bool isUsingEMP() {
    return usingEMP;
  }

  public bool isUsingDopple() {
    return usingDopple;
  }

  public bool isUsingTransformer() {
    return usingTransformer;
  }

  public bool isTrapped() {
    return trapped;
  }

  public void stopEMP() {
    if (!usingPowerBoost) {
      rb.isKinematic = false;
      rotatePlayerBody();
    }
    usingEMP = false;
    afterStrengthenStart();
  }

  public bool uncontrollable() {
    return isRebounding() || isUsingRainbow() || usingEMP || bouncing || bouncingByDispenser;
  }

  void Update() {
    if (isRotatingByRainbow) {
      Vector3 dir = (rainbowPosition - transform.position).normalized;
      transform.Translate(dir * Time.deltaTime * 30, Space.World);
      transform.Rotate(-Vector3.forward * Time.deltaTime * rdm.rotateAngularSpeed, Space.World);
    }

    if (afterStrengthen) {
      if (afterStrengthenCount < afterStrengthenDuration) {
        afterStrengthenCount += Time.deltaTime;
      } else {
        afterStrengthen = false;
      }
    }

    if (isBouncing()) {
      if (bounceDuration > 0) {
          bounceDuration -= Time.deltaTime;
      } else {
        if (reboundingByBlackhole) {
          reboundingByBlackhole = false;
          if (isRidingRainbowRoad) isRidingRainbowRoad = false;
          Camera.main.GetComponent<CameraMover>().stopShake();
          afterStrengthenStart();
        } else if (bouncingByDispenser) {
          bouncingByDispenser = false;
        } else if (bouncing) {
          bouncing = false;
        }
      }
    }

    if (iced) {
      if (icedDuration > 0) {
        icedDuration -= Time.deltaTime;
        icedSpeedFactor = Mathf.MoveTowards(icedSpeedFactor, 1, Time.deltaTime * (1 - icm.playerSpeedReduceTo) / icm.speedRestoreDuring);
      } else {
        iced = false;
        icm.strengthenPlayerEffect.SetActive(false);
      }
    }
  }

  public void destroyObject(string tag, int gaugeGain = 0) {
    numDestroyObstacles++;
    DataManager.dm.increment("TotalNumDestroyObstacles");

    if (tag == "Obstacle_small") {
      DataManager.dm.increment("NumDestroySmallAsteroids");
      SuperheatManager.sm.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Obstacle_big") {
      DataManager.dm.increment("NumDestroyAsteroids");
      SuperheatManager.sm.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Obstacle") {
      DataManager.dm.increment("NumDestroyMeteroids");
      SuperheatManager.sm.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Blackhole") DataManager.dm.increment("NumDestroyBlackholes");
    else if (tag == "Monster") DataManager.dm.increment("NumDestroyMonsters");
    else if (tag == "DangerousEMP") DataManager.dm.increment("NumDestroyDangerousEMP");

    if (!usingEMP) Camera.main.GetComponent<CameraMover>().shake();
  }

  public void encounterObject(string tag) {
    numUseObjects++;
    DataManager.dm.increment("TotalNumUseObjects");

    if (tag == "Jetpack") DataManager.dm.increment("NumUseJetpack");
    else if (tag == "Metal") DataManager.dm.increment("NumUseMetal");
    else if (tag == "Monster") DataManager.dm.increment("NumRideMonster");
    else if (tag == "Dopple") DataManager.dm.increment("NumMeetDopple");
    else if (tag == "ComboPart") DataManager.dm.increment("NumGenerateIllusion");
    else if (tag == "CubeDispenser") DataManager.dm.increment("NumBumpCubeDispenser");
    else if (tag == "SummonPart") DataManager.dm.increment("NumSummon");
    else if (tag == "RainbowDonut") DataManager.dm.increment("NumRideRainbow");
    else if (tag == "EMP") DataManager.dm.increment("NumGenerateForcefield");
    else if (tag == "Magnet") DataManager.dm.increment("Magnet");
    else if (tag == "Transformer") DataManager.dm.increment("Transformer");
    else Debug.LogError("Exception? " + tag);
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestBoosters", numBoosters);
    DataManager.dm.setBestInt("BestNumDestroyObstacles", numDestroyObstacles);
    DataManager.dm.setBestInt("BestNumUseObjects", numUseObjects);
  }

  public bool isInvincible() {
    return afterStrengthen || ridingMonster || unstoppable || isRebounding() || isUsingRainbow() || changeManager.isTeleporting();
  }

  public void generateGoldCube(ObjectsMover obj) {
    string tag = obj.tag;
    Vector3 position = obj.transform.position;

    int random = Random.Range(0, 200);
    if ((random < 4 && tag == "Obstacle") || (random < 1 && (tag == "Obstacle_small" || tag == "Obstacle_big"))) {
      GameObject cube = generateCube();
      cube.transform.position = position;
      cube.GetComponent<Renderer>().sharedMaterial = goldenCubeMat;
      cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
      gcCount.add(10);
      cube.SetActive(true);
      cube.transform.localScale += 20 * bonusCubeScaleChange * Vector3.one;
    }
  }

  public bool canBeMagnetized() {
    return !(isRebounding() || isUsingRainbow() || changeManager.isTeleporting());
  }

  public void fasterSpeed(int val) {
    baseSpeed *= (100 + val) / 100f;
  }

  public void moreSpeedOn(int val, string condition) {
    speedScale *= (100 + val) / 100f;
    moreSpeedCondition = condition;
  }

  public void resetAbility() {
    baseSpeed = originalBaseSpeed;
    speedScale = 1;
    moreSpeedCondition = "";
  }
}
