using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
  public Transform effects;
  public SpawnManager spawnManager;

  public CubesCount cubesCount;
  public ScoreManager scoreManager;
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
  public JetpackManager jpm;

  public BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private bool exitedBlackhole = false;
  private bool reboundingByBlackhole = false;

  private bool reboundingByDispenser = false;
  private float reboundingByDispenserDuring = 0;

  public EnergyBar energyBar;
  private ComboBar comboBar;
  private StrengthenTimeBar stBar;

  public float boosterSpeedUpAmount = 60;
  public float maxBoosterSpeed = 100;
  public float boosterSpeedDecreaseBase = 70;
  public float boosterSpeedDecreasePerTime = 20;
  private float boosterBonus = 1;

	public GameObject particles;

  public int strengthen_during = 8;
  public float reboundSpeed = 300;

  private bool unstoppable = false;
  private bool usingEMP = false;
  private bool ridingMonster = false;
  private bool usingJetpack = false;
  private bool usingDopple = false;
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

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    speed = comboBar.getSpeed();

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;
    rotatePlayerBody(true);
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
      Vector3 heading = blm.instance.transform.position - transform.position;
      if (heading.magnitude < 1) rb.isKinematic = true;
      else {
        heading /= heading.magnitude;
        rb.AddForce(heading * blm.pullUser, ForceMode.VelocityChange);
      }
    }

    if (usingDopple) {
      rb.velocity = Vector3.zero;
    } else {
      rb.velocity = direction * speed;
    }
	}

	void OnTriggerEnter(Collider other) {
    if (other.tag == "BlackholeGravitySphere") {
      isInsideBlackhole = true;
      return;
    }

    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();

    if (mover.dangerous()) {
      scoreManager.gameOver();
      return;
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
    mover.destroyObject();
    mover.destroyByMonster();
    for (int k = 0; k < monm.numMinimonSpawn; k++) {
      Instantiate(monm.minimonPrefab, transform.position, transform.rotation);
    }
  }

  public void goodPartsEncounter(ObjectsMover mover, int howMany, int bonus = 0) {
    if (ridingMonster && mover.tag != "MiniMonster" && mover.tag != "RainbowDonut") {
      generateMinimon(mover);
      return;
    }

    if (mover.tag == "MiniMonster") {
      if (!absorbMinimon(mover)) return;
    }

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

      if (mover.tag != "GoldenCube") addCubeCount(howMany, bonus);
    }

    mover.encounterPlayer();
  }

  public void addCubeCount(int howMany = 1, int bonus = 0) {
    cubesCount.addCount(howMany, bonus);
    energyBar.getHealthbyParts(howMany + bonus);
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring) {
    for (int e = 0; e < howMany; e++) {
      Instantiate(particles, tr.position, tr.rotation);
    }

    addCubeCount(howMany, 0);

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
      rb.angularVelocity = Random.onUnitSphere * tumble;
    }
  }

  public Vector3 getDirection() {
    return direction;
  }

  public void teleport(Vector3 pos) {
    if (changeManager.isTeleporting()) return;

    // energyBar.loseByTeleport();
    changeManager.teleport(pos);
    cpm.tryToGet();
  }

  public void shootBooster(){
    QuestManager.qm.addCountToQuest("NoBooster", 0);
    QuestManager.qm.addCountToQuest("UseBooster");

    if (usingJetpack) {
      QuestManager.qm.addCountToQuest("Jetpack");
    }

    energyBar.loseByShoot();

    changeManager.booster.Play();
    changeManager.booster.GetComponent<AudioSource>().Play();

    if (boosterspeed < maxBoosterSpeed * boosterBonus) {
      boosterspeed += boosterSpeedUpAmount * boosterBonus;
      boosterspeed = boosterspeed > (maxBoosterSpeed * boosterBonus)? (maxBoosterSpeed * boosterBonus) : boosterspeed;
    }

    cpm.tryToGet();
  }

  public void setRotateByRainbow(bool val) {
    rainbowPosition = rdm.instance.transform.position;
    rb.isKinematic = val;
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

  public void strengthenBy(string obj) {
    if (obj == "SpecialPart") {
      unstoppable = true;
    } else if (obj == "Blackhole") {
      isInsideBlackhole = false;
      exitedBlackhole = true;
    } else if (obj == "Monster") {
      ridingMonster = true;
      minimonCounter = 0;
      energyBar.getFullHealth();
    } else if (obj == "EMP") {
      usingEMP = true;
      rb.isKinematic = true;
      return;
    } else if (obj == "Jetpack") {
      usingJetpack = true;
      energyBar.setCharged(true);
      // showEffect("Jetpack");
      boosterBonus = jpm.boosterBonusScale;
      changeManager.booster.GetComponent<ParticleSystem>().emissionRate *= boosterBonus;
    } else if (obj == "Dopple") {
      usingDopple = true;
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

    stBar.startStrengthen(effectDuration);

    yield return new WaitForSeconds(effectDuration);

    stopStrengthen();
  }

  public void stopStrengthen() {
    if (usingJetpack) {
      usingJetpack = false;
      changeManager.booster.GetComponent<ParticleSystem>().emissionRate /= boosterBonus;
      boosterBonus = 1;
      spawnManager.runManager("Jetpack");
    }

    if (unstoppable) {
      unstoppable = false;
      spawnManager.runManager("SpecialParts");

      QuestManager.qm.addCountToQuest("DestroyAsteroidsBeforeUnstoppableEnd", 0);

      afterStrengthenStart();
    }

    if (reboundingByBlackhole) {
      reboundingByBlackhole = false;
      if (isRidingRainbowRoad) {
        isRidingRainbowRoad = false;
      }

      afterStrengthenStart();
    }

    if (exitedBlackhole) {
      exitedBlackhole = false;
      spawnManager.runManager("Blackhole");
    }

    if (ridingMonster) {
      energyBar.getFullHealth();
      ridingMonster = false;
      monm.stopRiding();
      transform.localScale = originalScale * Vector3.one;
      spawnManager.runManager("Monster");

      afterStrengthenStart();
    }

    if (usingDopple) {
      usingDopple = false;
      Camera.main.GetComponent<CameraMover>().setSlowly(false);
      spawnManager.runManager("Dopple");
    }
  }

  public void contactBlackhole(Collision collision) {
    if (unstoppable) {
      QuestManager.qm.addCountToQuest("ReboundByBlackhole");
    } else if (isUsingRainbow()) {
      rdm.destroyInstances();
    }

    processCollision(collision);
    reboundingByBlackhole = true;
  }

  public void afterStrengthenStart() {
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

  public void stopEMP() {
    rb.isKinematic = false;
    usingEMP = false;
    rotatePlayerBody();
    showEffect("Charged");
    QuestManager.qm.addCountToQuest("DestroyAsteroidAndFallingStarByEMP", 0);
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
}
