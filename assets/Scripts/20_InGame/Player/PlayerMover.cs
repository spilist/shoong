using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
  public Transform effects;
  public SpawnManager spawnManager;

  public CubesCount cubesCount;
  public ScoreManager scoreManager;
  public GameObject obstacleDestroy;
  public GameObject getBlackhole;
  public int cubesAsItIsUntill = 20;
  public int restCubesChangePer = 5;
  public int nearAsteroidBonus = 10;
  public float bonusCubeScaleChange = 0.2f;
  public float bonusCubeMaxBase = 100;

  private float speed;
	private float boosterspeed = 0;
  public float tumble;
  private Vector3 direction;

  public ComboPartsManager cpm;
  public MonsterManager monm;
  public RainbowDonutsManager rdm;

  public BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private bool exitedBlackhole = false;
  private bool reboundingByBlackhole = false;

  private bool reboundingByDispenser = false;
  private float reboundingByDispenserDuring = 0;

  private EnergyBar energyBar;
  private ComboBar comboBar;
  private StrengthenTimeBar stBar;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;
  public float boosterSpeedDecreaseBase = 70;
  public float boosterSpeedDecreasePerTime = 20;

	public GameObject particles;

  public int strengthen_during = 8;
  public float reboundSpeed = 300;

  private bool unstoppable = false;

  private bool ridingMonster = false;
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

	void Start () {
    changeManager = GetComponent<CharacterChangeManager>();
    changeManager.changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    originalScale = transform.localScale.x;
    energyBar = transform.parent.Find("Bars Canvas/EnergyBar").GetComponent<EnergyBar>();
    comboBar = transform.parent.Find("Bars Canvas").GetComponent<ComboBar>();
    stBar = transform.parent.Find("Bars Canvas/StrengthenTimeBar").GetComponent<StrengthenTimeBar>();

    rotatePlayerBody(true);

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    speed = comboBar.getSpeed();

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;

    changeManager.getBlackholeEffect.transform.localScale = Vector3.one * getBlackhole.GetComponent<GetBlackhole>().radiusPerLevel[DataManager.dm.getInt("BlackholeLevel") - 1] / getBlackhole.GetComponent<GetBlackhole>().radiusPerLevel[1];

	}

	void FixedUpdate () {
    if (isRidingRainbowRoad) {
      speed = rdm.ridingSpeed;
    } else if (isRebounding()) {
      speed = reboundSpeed;
    } else if (ridingMonster) {
      speed = comboBar.getSpeed() + minimonCounter * monm.enlargeSpeedPerMinimon;
    } else {
      speed = comboBar.getSpeed();
    }

    speed += boosterspeed;

    if (boosterspeed > 0) {
      boosterspeed -= speed / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.deltaTime;
    } else if (boosterspeed <= 0){
      boosterspeed = 0;
    }

    if (isInsideBlackhole && !isUsingRainbow()) {
      Vector3 heading = blackhole.transform.position - transform.position;
      if (heading.magnitude < 1) rb.isKinematic = true;
      else {
        heading /= heading.magnitude;
        rb.AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
      }
    }
    rb.velocity = direction * speed;
	}

	void OnTriggerEnter(Collider other) {
    if (other.tag == "BlackholeGravitySphere" || other.tag == "Blackhole") return;

    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (mover.dangerous()) {
      scoreManager.gameOver();
      return;
    }

    if (!unstoppable && mover.tag == "Monster" && ((MonsterMover)mover).rideable()) {
      startRiding(mover);
      return;
    }

    goodPartsEncounter(mover, mover.cubesWhenEncounter(), mover.bonusCubes());
	}

  public void goodPartsEncounter(ObjectsMover mover, int howMany, int bonus = 0) {
    if (ridingMonster && mover.tag != "MiniMonster" && mover.tag != "RainbowDonut") {
      mover.destroyObject();
      mover.destroyByMonster();
      for (int k = 0; k < monm.numMinimonSpawn; k++) {
        Instantiate(monm.minimonPrefab, transform.position, transform.rotation);
      }
      return;
    }

    if (mover.tag == "MiniMonster") {
      if (!((MiniMonsterMover)mover).isTimeElapsed()) return;

      if (minimonCounter < monm.maxEnlargeCount) {
        minimonCounter++;
        transform.localScale += monm.enlargeScalePerMinimon * Vector3.one;
      }
    }

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
        cube.GetComponent<Renderer>().material.SetColor("_TintColor", rdm.rainbowColors[e]);
        cube.GetComponent<ParticleMover>().setRainbow();
      }
    }

    if (bonus > 0) {
      GameObject cube = (GameObject) Instantiate(particles, mover.transform.position, mover.transform.rotation);
      cube.GetComponent<ParticleMover>().triggerCubesGet(bonus);
      if (unstoppable && mover.isNegativeObject()) {
        cube.GetComponent<Renderer>().sharedMaterial = changeManager.metalMat;
      }

      cube.transform.localScale += Mathf.Min(bonus, bonusCubeMaxBase) * bonusCubeScaleChange * Vector3.one;
    }

    if (mover.tag != "GoldenCube") addCubeCount(howMany, bonus, true);

    mover.encounterPlayer();
  }

  public void addCubeCount(int howMany = 1, int bonus = 0, bool comboAndEnergy = false) {
    cubesCount.addCount(howMany, bonus);
    energyBar.getHealthbyParts(howMany + bonus);

    if (comboAndEnergy) {
      changeManager.getEnergy.Play ();
      comboBar.addCombo();
    }
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring) {
    for (int e = 0; e < howMany; e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }

    addCubeCount(howMany, 0, true);

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
      GetComponent<Rigidbody>().angularVelocity = new Vector3(float.Parse(angVals[0]), float.Parse(angVals[1]), float.Parse(angVals[2]));
    } else {
      GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
    }
  }

  public Vector3 getDirection() {
    return direction;
  }

  public void shootBooster(Vector3 dir){
    QuestManager.qm.addCountToQuest("NoBooster", 0);
    QuestManager.qm.addCountToQuest("UseBooster");

    energyBar.loseByShoot();

    rotatePlayerBody();

    changeManager.booster.Play();
    changeManager.booster.GetComponent<AudioSource>().Play();

    direction = dir;

    if (boosterspeed < maxBoosterSpeed) {
      boosterspeed += boosterSpeedUpAmount;
      boosterspeed = boosterspeed > maxBoosterSpeed? maxBoosterSpeed : boosterspeed;
    }

    cpm.tryToGet();
  }

  public void setRotateByRainbow(bool val) {
    rainbowPosition = rdm.rainbowDonut.transform.position;
    GetComponent<Rigidbody>().isKinematic = val;
    isRotatingByRainbow = val;
  }

  public void setDirection(Vector3 dir) {
    direction = dir;
    rotatePlayerBody();
  }

  public void nearAsteroid(bool enter = true, int amount = 1) {
    if (enter) nearAsteroidCounter += amount;
    else nearAsteroidCounter -= amount;
  }

  public bool isNearAsteroid() {
    return nearAsteroidCounter > 0;
  }

  public void showEffect(string effectName) {
    if (scoreManager.isGameOver()) return;

    if (effectName == "Whew") {
      boosterspeed += 140;
      changeManager.booster.Play();
      afterStrengthenStart();
      // audio needed
    } else if (effectName == "Charged") {
      energyBar.setCharged();
    }

    effects.Find(effectName).gameObject.SetActive(true);
  }

  public IEnumerator strengthen() {
    if (scoreManager.isGameOver()) yield break;

    int effectDuration;
    if (reboundingByBlackhole) {
      effectDuration = blm.reboundDuring;
      getBlackhole.SetActive(true);
    } else {
      effectDuration = strengthen_during;
    }

    if (unstoppable) {
      showEffect("Metal");
      changeManager.changeCharacterToMetal();
    }

    if (exitedBlackhole) {
      showEffect("Blackhole");
      getBlackhole.SetActive(true);
      changeManager.getBlackholeEffect.Play();
      changeManager.getBlackholeEffect.GetComponent<AudioSource>().Play();
    }

    if (ridingMonster) {
      showEffect("Monster");
      energyBar.getFullHealth();
      changeManager.changeCharacterToMonster();
    }

    stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    stopStrengthen();
  }

  public void stopStrengthen() {
    if (unstoppable) {
      spawnManager.runManager("SpecialParts");
      unstoppable = false;
      changeManager.unstoppableEffect.Stop();
      // unstoppableEffect_two.Stop();
      changeManager.unstoppableEffect_two.GetComponent<AudioSource>().Stop ();
      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd", 0);
      changeManager.changeCharacterToOriginal();

      afterStrengthenStart();
    }

    if (reboundingByBlackhole) {
      reboundingByBlackhole = false;
      getBlackhole.SetActive(false);
      if (isRidingRainbowRoad) {
        isRidingRainbowRoad = false;
        changeManager.rainbowEffect.Stop();
      }

      afterStrengthenStart();
    }

    if (exitedBlackhole) {
      exitedBlackhole = false;
      getBlackhole.SetActive(false);
      blm.run();
    }

    if (ridingMonster) {
      energyBar.getFullHealth();
      ridingMonster = false;
      changeManager.changeCharacterToOriginal();
      changeManager.ridingEffect.Play();
      changeManager.monsterEffect.Stop();
      changeManager.monsterEffect.GetComponent<AudioSource>().Stop();
      monm.monsterFilter.SetActive(false);
      foreach (GameObject mm in GameObject.FindGameObjectsWithTag("MiniMonster")) {
        mm.GetComponent<MiniMonsterMover>().destroyObject();
      }
      transform.localScale = originalScale * Vector3.one;
      monm.run();

      afterStrengthenStart();
    }
  }

  public void startRiding(ObjectsMover obMover) {
    QuestManager.qm.addCountToQuest("RideMonster");

    if (energyBar.currentEnergy() <= 30) {
      QuestManager.qm.addCountToQuest("RideMonsterWithLowEnergy");
    }

    if (exitedBlackhole) {
      QuestManager.qm.addCountToQuest("RideMonsterByBlackhole");
    }

    ridingMonster = true;
    minimonCounter = 0;
    monm.monsterFilter.SetActive(true);
    changeManager.ridingEffect.Play();
    changeManager.ridingEffect.GetComponent<AudioSource>().Play();
    Destroy(obMover.gameObject);
    monm.stopWarning();
    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void startUnstoppable() {
    unstoppable = true;
    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }

  public void outsideBlackhole() {
    QuestManager.qm.addCountToQuest("ExitBlackhole");

    isInsideBlackhole = false;
    exitedBlackhole = true;
    Destroy(blackhole);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileUnstoppable(Collision collision) {
    QuestManager.qm.addCountToQuest("ReboundByBlackhole");

    reboundingByBlackhole = true;
    isInsideBlackhole = false;
    processCollision(collision);

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void contactBlackholeWhileRainbow(Collision collision) {
    reboundingByBlackhole = true;
    isInsideBlackhole = false;
    processCollision(collision);
    rdm.destroyInstances();

    StopCoroutine("strengthen");
    StartCoroutine("strengthen");
  }

  public void afterStrengthenStart() {
    afterStrengthen = true;
    afterStrengthenCount = 0;
    changeManager.afterStrengthenEffect.Play();
  }

  public bool isAfterStrengthen() {
    return afterStrengthen;
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

  public bool isRebounding() {
    return reboundingByBlackhole || reboundingByDispenser;
  }

  public void setRidingRainbowRoad(bool val) {
    isRidingRainbowRoad = val;
  }

  public bool isUsingRainbow() {
    return isRotatingByRainbow || isRidingRainbowRoad;
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
}
