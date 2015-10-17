using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
  public float baseSpeed = 80;

  public Transform effects;
  public SpawnManager spawnManager;
  public GoldCubesCount gcCount;

  public CubesCount cubesCount;
  public ScoreManager scoreManager;
  public int cubesAsItIsUntill = 20;
  public int restCubesChangePer = 5;
  public int nearAsteroidBonus = 10;
  public float bonusCubeScaleChange = 0.2f;
  public float bonusCubeMaxBase = 100;

  private float speed;
	private float boosterspeed = 0;
  public float minTumble;
  private float tumble;
  private Vector3 direction;

  public ComboPartsManager cpm;
  public MonsterManager monm;
  public RainbowDonutsManager rdm;
  public JetpackManager jpm;
  public DoppleManager dpm;

  public BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private bool exitedBlackhole = false;
  private bool reboundingByBlackhole = false;

  private bool reboundingByDispenser = false;
  private float reboundingByDispenserDuring = 0;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;
  public float boosterSpeedDecreaseBase = 70;
  public float boosterSpeedDecreasePerTime = 20;
  private float boosterBonus = 1;

	public GameObject particles;
  public Color goldenCubeParticleColor;
  public Color goldenCubeParticleTrailColor;

  public int strengthen_during = 8;
  public float reboundSpeed = 300;

  private bool unstoppable = false;
  private bool usingEMP = false;
  private bool ridingMonster = false;
  private bool usingJetpack = false;
  private bool usingDopple = false;
  private bool usingMagnet = false;
  private float originalScale;
  private int minimonCounter = 0;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;
  private Rigidbody rb;

  private int nearAsteroidCounter = 0;

  public float afterStrengthenDuration = 1;
  private bool afterStrengthen = false;
  private float afterStrengthenCount = 0;
  private float reboundingCount = 0;

  private CharacterChangeManager changeManager;
  public Collider contactCollider;
  public GameObject playerDopple;
  private bool trapped = false;

  public Superheat superheat;
  private bool usingPowerBoost = false;

  private int numBoosters = 0;
  private int numDestroyObstacles = 0;
  private int numUseObjects = 0;

  // private int magnetStatus = 0;
  // private Transform magnetOrigin;
  // private int magnetPower;

	void Start () {
    changeManager = GetComponent<CharacterChangeManager>();
    changeManager.changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    originalScale = transform.localScale.x;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    speed = baseSpeed;

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;
    rotatePlayerBody(true);
	}

	void FixedUpdate () {
    if (usingPowerBoost) {
      speed = superheat.baseSpeed;
    } else if (usingEMP) {
      speed = 0;
    } else if (isRidingRainbowRoad) {
      speed = rdm.ridingSpeed;
    } else if (isRebounding()) {
      speed = reboundSpeed;
    } else if (ridingMonster) {
      speed = baseSpeed + minimonCounter * monm.enlargeSpeedPerMinimon;
    } else {
      speed = baseSpeed;
    }

    speed += boosterspeed;

    if (boosterspeed > 0) {
      boosterspeed -= speed / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.deltaTime;
    } else if (boosterspeed <= 0){
      boosterspeed = 0;
    }

    if (isInsideBlackhole && !isUsingRainbow() && !usingDopple) {
      Vector3 heading = blm.instance.transform.position - transform.position;
      if (heading.magnitude < 1) rb.isKinematic = true;
      else {
        heading /= heading.magnitude;
        rb.AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
      }
    }
    // else if (magnetStatus != 0 && !isUsingRainbow() && !usingDopple) {
    //   Vector3 heading = magnetOrigin.position - transform.position;
    //   if (heading.magnitude < 1) rb.isKinematic = true;
    //   else {
    //     heading /= heading.magnitude;
    //     int force = magnetStatus == 1 ? magnetPower : -magnetPower;
    //     rb.AddForce(heading * force, ForceMode.VelocityChange);
    //   }
    // }

    if (!isRidingRainbowRoad && (usingDopple || trapped)) {
      rb.velocity = Vector3.zero;
    } else {
      rb.velocity = direction * speed;
    }
	}

  public float getSpeed() {
    return rb.velocity.magnitude;
  }

	void OnTriggerEnter(Collider other) {
    if (other.tag == "BlackholeGravitySphere") {
      isInsideBlackhole = true;
      return;
    }

    if (other.tag == "MagnetArea") return;

    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (mover == null) return;

    if (mover.dangerous()) {
      scoreManager.gameOver(mover.tag);
      return;
    }

    if (ridingMonster && mover.tag != "MiniMonster" && mover.tag != "RainbowDonut") {
      generateMinimon(mover);
      return;
    }

    if (mover.tag == "MiniMonster") {
      if (!absorbMinimon(mover)) return;
    }

    if (mover.tag == "CubeDispenser") {
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
    mover.destroyByMonster();
    for (int k = 0; k < monm.numMinimonSpawn; k++) {
      Instantiate(monm.minimonPrefab, transform.position, transform.rotation);
    }
    DataManager.dm.increment("NumSpawnMinimonster", monm.numMinimonSpawn);
  }

  public void goodPartsEncounter(ObjectsMover mover, int howMany, int bonus = 0) {
    if (howMany > 0) {
      int instantiateCount;
      if (howMany < cubesAsItIsUntill) {
        instantiateCount = howMany;
      } else {
        instantiateCount = cubesAsItIsUntill + Mathf.RoundToInt((howMany - cubesAsItIsUntill) / (float) restCubesChangePer);
      }

      for (int e = 0; e < instantiateCount; e++) {
        GameObject cube = (GameObject) Instantiate(particles, mover.transform.position, mover.transform.rotation);
        if (e == 0) {
          cube.GetComponent<ParticleMover>().triggerCubesGet(howMany);
        }
        if (mover.tag == "RainbowDonut") {
          if (rdm.isGolden) {
            cube.GetComponent<Renderer>().material.SetColor("_TintColor", goldenCubeParticleColor);
            cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
          } else {
            cube.GetComponent<Renderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
            cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
          }

          cube.GetComponent<ParticleMover>().setRainbow();
        }
      }

      if (mover.isNegativeObject()) {
        if (isUsingRainbow()) DataManager.dm.increment("NumDestroyObstaclesOnRainbow");
        else if (unstoppable) DataManager.dm.increment("NumDestroyObstaclesWithMetal");

        generateGoldCube(mover);
      }

      if (bonus > 0) {
        GameObject cube = (GameObject) Instantiate(particles, mover.transform.position, mover.transform.rotation);
        cube.GetComponent<ParticleMover>().triggerCubesGet(bonus);
        if (unstoppable && mover.isNegativeObject()) {
          cube.GetComponent<Renderer>().sharedMaterial = changeManager.metalMat;
          DataManager.dm.increment("TotalBonusesWithMetal", bonus);
        }

        cube.transform.localScale += Mathf.Min(bonus, bonusCubeMaxBase) * bonusCubeScaleChange * Vector3.one;
      }

      if (mover.tag != "GoldenCube") addCubeCount(howMany, bonus);
    }

    mover.encounterPlayer();
  }

  public void addCubeCount(int howMany = 1, int bonus = 0) {
    cubesCount.addCount(howMany, bonus);
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring, bool isGolden) {
    for (int e = 0; e < howMany; e++) {
      GameObject cube = (GameObject) Instantiate(particles, tr.position, tr.rotation);
      if (isGolden) {
        cube.GetComponent<Renderer>().material.SetColor("_TintColor", goldenCubeParticleColor);
        cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
      } else if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(howMany);
      }
    }

    if (!isGolden) addCubeCount(howMany, 0);

    processCollision(collision);
    reboundingByDispenser = true;
    reboundingCount = 0;
    reboundingByDispenserDuring = reboundDuring;
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
      tumble = (cubesCount.getCPS() > minTumble) ? cubesCount.getCPS() : minTumble;
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
    if (changeManager.isTeleporting() || scoreManager.isGameOver()) return;

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
    cpm.tryToGet();

  }

  public void shootBooster(){
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

    cpm.tryToGet();

    numBoosters++;
    DataManager.dm.increment("TotalBoosters");
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
    isInsideBlackhole = false;
    changeManager.changeCharacterToOriginal();

    if (isUsingRainbow()) {
      rdm.destroyInstances();
      isRotatingByRainbow = false;
      isRidingRainbowRoad = false;
    }
    stopStrengthen();
  }

  public void stopPowerBoost() {
    usingPowerBoost = false;
    GetComponent<Renderer>().enabled = true;
    GetComponent<Collider>().enabled = true;
    rotatePlayerBody();
    afterStrengthenStart();
  }

  public float maxBooster() {
    if (usingPowerBoost) return superheat.maxBoosterSpeed;
    else return maxBoosterSpeed * boosterBonus;
  }

  float boosterSpeedUp() {
    if (usingPowerBoost) return superheat.boosterSpeedUpAmount;
    else return boosterSpeedUpAmount;
  }

  public void setRotateByRainbow(bool val) {
    rainbowPosition = rdm.instance.transform.position;
    rb.isKinematic = val;
    isRotatingByRainbow = val;
  }

  public void setDirection(Vector3 dir) {
    direction = dir;
    if (usingPowerBoost) superheat.setDir(dir);
    rotatePlayerBody();
  }

  public void nearAsteroid(bool enter = true, int amount = 1) {
    if (enter) nearAsteroidCounter += amount;
    else nearAsteroidCounter -= amount;
  }

  public bool isNearAsteroid() {
    return nearAsteroidCounter > 0;
  }

  public void showEffect(string effectName, int scale = 1) {
    if (usingPowerBoost || scoreManager.isGameOver()) return;

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

  public void strengthenBy(string obj) {
    if (usingPowerBoost) {
      changeManager.changeCharacterToOriginal();
      return;
    }

    if (obj == "SpecialPart") {
      unstoppable = true;
    } else if (obj == "Magnet") {
      usingMagnet = true;
    } else if (obj == "Blackhole") {
      isInsideBlackhole = false;
      exitedBlackhole = true;
    } else if (obj == "Monster") {
      ridingMonster = true;
      minimonCounter = 0;
    } else if (obj == "EMP") {
      usingEMP = true;
      rb.isKinematic = true;
      return;
    } else if (obj == "Jetpack") {
      usingJetpack = true;
      boosterBonus = jpm.boosterBonusScale;
      changeManager.booster.GetComponent<ParticleSystem>().emissionRate *= (boosterBonus * 2);
      changeManager.booster.transform.localScale = (boosterBonus * 2) * Vector3.one;
    } else if (obj == "Dopple") {
      usingDopple = true;
      contactCollider.enabled = false;
    }

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }


  public IEnumerator strengthen() {
    if (scoreManager.isGameOver()) yield break;

    int effectDuration;
    if (reboundingByBlackhole) {
      effectDuration = blm.reboundDuring;
    } else {
      effectDuration = strengthen_during;
    }

    // stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    stopStrengthen();
  }

  public void stopStrengthen() {
    if (usingJetpack) {
      usingJetpack = false;
      changeManager.booster.GetComponent<ParticleSystem>().emissionRate /= (boosterBonus * 2);
      changeManager.booster.transform.localScale = Vector3.one;
      boosterBonus = 1;
      spawnManager.runManager("Jetpack");
    }

    if (unstoppable) {
      unstoppable = false;
      spawnManager.runManager("SpecialParts");
      afterStrengthenStart();
    }

    if (reboundingByBlackhole) {
      reboundingByBlackhole = false;
      if (isRidingRainbowRoad) {
        isRidingRainbowRoad = false;
      }
      Camera.main.GetComponent<CameraMover>().stopShake();
      afterStrengthenStart();
    }

    if (exitedBlackhole) {
      exitedBlackhole = false;
      spawnManager.runManager("Blackhole");
    }

    if (usingMagnet) {
      usingMagnet = false;
      spawnManager.runManager("Magnet");
    }

    if (ridingMonster) {
      ridingMonster = false;
      monm.stopRiding();
      transform.localScale = originalScale * Vector3.one;
      spawnManager.runManager("Monster");

      afterStrengthenStart();
    }

    if (usingDopple) {
      usingDopple = false;
      if (!trapped) contactCollider.enabled = true;
      Camera.main.GetComponent<CameraMover>().setSlowly(false);
      spawnManager.runManager("Dopple");
      afterStrengthenStart();
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

  public bool isUsingJetpack() {
    return usingJetpack;
  }

  public bool isUnstoppable() {
    return unstoppable;
  }

  public bool isRidingMonster() {
    return ridingMonster;
  }

  public bool isUsingBlackhole() {
    return exitedBlackhole || isInsideBlackhole;
  }

  public bool isExitedBlackhole() {
    return exitedBlackhole;
  }

  public bool isUsingMagnet() {
    return usingMagnet;
  }

  public bool isRebounding() {
    return reboundingByBlackhole || reboundingByDispenser;
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

  public bool isTrapped() {
    return trapped;
  }

  public void stopEMP() {
    rb.isKinematic = false;
    usingEMP = false;
    rotatePlayerBody();
    afterStrengthenStart();
  }

  public bool uncontrollable() {
    return isRebounding() || isUsingRainbow() || usingEMP;
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

    if (reboundingByDispenser) {
      if (reboundingCount < reboundingByDispenserDuring) {
        reboundingCount += Time.deltaTime;
      } else {
        reboundingByDispenser = false;
      }
    }
  }

  public void destroyObject(string tag, int gaugeGain = 0) {
    numDestroyObstacles++;
    DataManager.dm.increment("TotalNumDestroyObstacles");

    if (tag == "Obstacle_small") {
      DataManager.dm.increment("NumDestroySmallAsteroids");
      superheat.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Obstacle_big") {
      DataManager.dm.increment("NumDestroyAsteroids");
      superheat.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Obstacle") {
      DataManager.dm.increment("NumDestroyMeteroids");
      superheat.addGuageWithEffect(gaugeGain);
    }
    else if (tag == "Blackhole") DataManager.dm.increment("NumDestroyBlackholes");
    else if (tag == "Monster") DataManager.dm.increment("NumDestroyMonsters");

    if (!usingEMP) Camera.main.GetComponent<CameraMover>().shake();
  }

  public void encounterObject(string tag) {
    numUseObjects++;
    DataManager.dm.increment("TotalNumUseObjects");

    if (tag == "Jetpack") DataManager.dm.increment("NumUseJetpack");
    else if (tag == "SpecialPart") DataManager.dm.increment("NumUseMetal");
    else if (tag == "Blackhole") DataManager.dm.increment("NumExitBlackhole");
    else if (tag == "Monster") DataManager.dm.increment("NumRideMonster");
    else if (tag == "Dopple") DataManager.dm.increment("NumMeetDopple");
    else if (tag == "ComboPart") DataManager.dm.increment("NumGenerateIllusion");
    else if (tag == "CubeDispenser") DataManager.dm.increment("NumBumpCubeDispenser");
    else if (tag == "SummonPart") DataManager.dm.increment("NumSummon");
    else if (tag == "RainbowDonut") DataManager.dm.increment("NumRideRainbow");
    else if (tag == "EMP") DataManager.dm.increment("NumGenerateForcefield");
    else if (tag == "Magnet") DataManager.dm.increment("Magnet");
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
      GameObject cube = (GameObject) Instantiate(particles, position, Quaternion.identity);
      cube.GetComponent<Renderer>().material.SetColor("_TintColor", goldenCubeParticleColor);
      cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", goldenCubeParticleTrailColor);
      gcCount.add(10);
      cube.transform.localScale += 10 * bonusCubeScaleChange * Vector3.one;
    }
  }

  // public void magnetized(bool pull, Transform magnetOrigin, int power) {
  //   magnetStatus = pull ? 1 : 2;
  //   this.magnetOrigin = magnetOrigin;
  //   magnetPower = power;
  // }

  // public void magnetizeEnd() {
  //   magnetStatus = 0;
  // }

  public bool canBeMagnetized() {
    return !(isRebounding() || isUsingRainbow() || changeManager.isTeleporting());
  }
}
