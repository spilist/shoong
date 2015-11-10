﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour {
  public static Player pl;
  public float sensitivity;
  public float stopSphereRadius = 0.3f;
  public float stickPushMaxSpeedAt = 0.8f;
  private float stickSpeedScale = 1;
  private bool stopping = false;
  public int stoppingSpeed = 10;

  public float baseSpeed;
  public float speed;
	private float boosterspeed;
  private float boosterSpeedUpAmount;
  private float maxBoosterSpeed;
  private float boosterSpeedDecreaseBase;
  public float boosterSpeedDecreasePerTime = 20;
  private float reboundScale;
  private float speedBoostScale = 1;
  private float maxSpeedBoostScale;
  private float speedBoostDuration = 0;
  private float speedBoostCount = 0;
  private float boosterBonus = 1;
  private bool speedBoosting = false;

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

  public float reboundSpeed = 300;

  private bool unstoppable = false;
  private bool usingEMP = false;
  private bool ridingMonster = false;
  private bool usingMagnet = false;
  private bool usingTransformer = false;
  private bool iced = false;
  public float originalScale;
  private int minimonCounter = 0;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;
  private Rigidbody rb;

  public float afterStrengthenDuration = 1;
  private bool afterStrengthen = false;
  private float afterStrengthenCount = 0;

  private CharacterChangeManager changeManager;

  private bool usingPowerBoost = false;

  private int numBoosters = 0;
  private int numDestroyObstacles = 0;
  private int numUseObjects = 0;
  public PlayerDirectionIndicator dirIndicator;
  private float timeSpaned;
  private bool scaleChanged = false;
  private bool feverTime = false;
  public ParticleSystem getEnergy;
  public GetPartsText getPartsText;
  public UseBoosterText useBoosterText;

	void Awake() {
    pl = this;
    originalScale = transform.localScale.x;
  }

  void Start () {
    changeManager = GetComponent<CharacterChangeManager>();
    CharacterManager.cm.changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);
    setDirection(direction);
    speed = baseSpeed;

    rb = GetComponent<Rigidbody>();
    rb.velocity = direction * speed;
    rotatePlayerBody(true);
	}

	void FixedUpdate () {
    if (usingPowerBoost) {
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
    } else {
      if (stopping) {
        speed = Mathf.MoveTowards(speed, 0, Time.fixedDeltaTime * stoppingSpeed);
      } else if (ridingMonster) {
        speed = baseSpeed + minimonCounter * monm.enlargeSpeedPerMinimon + boosterspeed;
      } else {
        speed = baseSpeed + boosterspeed;
      }
    }

    if (speedBoosting) {
      if (speedBoostCount < speedBoostDuration) {
        speedBoostCount += Time.fixedDeltaTime;
        speedBoostScale = Mathf.MoveTowards(speedBoostScale, 1, Time.fixedDeltaTime * maxSpeedBoostScale / speedBoostDuration);
        speed *= speedBoostScale;
      } else {
        speedBoosting = false;
      }
    }

    if (iced && !uncontrollable() && !bouncing) {
      speed *= icedSpeedFactor;
    }
    if (boosterspeed > 0) {
      timeSpaned += Time.fixedDeltaTime;
      boosterspeed -= 80 / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.fixedDeltaTime;
      // Debug.Log("End: " + timeSpaned);
    } else if (boosterspeed <= 0){
      boosterspeed = 0;
    }

    rb.velocity = direction * speed * stickSpeedScale;
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

    if (tag == "TutorialPart") {
      other.gameObject.SetActive(false);
      getEnergy.Play();
      getEnergy.GetComponent<AudioSource>().Play();
      getPartsText.increment();
      return;
    }

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

    goodPartsEncounter(mover, mover.cubesWhenEncounter());
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

  public void goodPartsEncounter(ObjectsMover mover, int howMany, bool encounterPlayer = true) {

    if (mover.tag != "GoldenCube" && howMany > 0) addCubeCount(howMany);

    if (encounterPlayer) mover.encounterPlayer();
    else mover.destroyObject(true, true);
  }

  public void addCubeCount(int howMany = 1) {
    CubeManager.cm.addCount(howMany);
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring) {
    addCubeCount(howMany);
    processCollision(collision);
    bouncingByDispenser = true;
    this.bounceDuration = reboundDuring;
  }

  public void bounce(float bounceDuration, Collision collision) {
    if (bounceDuration == 0) return;

    processCollision(collision);
    bouncing = true;
    this.bounceDuration = bounceDuration * reboundScale;
  }

  public void loseEnergy(int amount, string tag) {
    Camera.main.GetComponent<CameraMover>().shake(shakeDuring * reboundScale, shakeBase * reboundScale * amount / 100);
    changeManager.changeRed();
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

  public void teleport(Vector3 pos) {
    if (changeManager.isTeleporting() || ScoreManager.sm.isGameOver()) return;

    AudioSource.PlayClipAtPoint(dpm.teleportSound, transform.position, dpm.teleportSoundVolume);

    changeManager.teleport(pos);
    DataManager.dm.increment("TotalBlinks");
  }

  public void stopMoving(bool val = true) {
    stopping = val;
    stickSpeedScale = 1;
  }

  public void shootBooster(){
    if (stopping || uncontrollable()) return;

    if (RhythmManager.rm.canBoost()) {
      RhythmManager.rm.ringSuccessed();

      if (!DataManager.dm.getBool("TutorialDone")) {
        useBoosterText.increment();
      }

      if (RhythmManager.rm.isSkillOK) {
        SkillManager.sm.activate();
        if (SkillManager.sm.isBlink()) {
          teleport(transform.position + direction * dpm.blinkDistance);
          return;
        }
      }
    } else {
      RhythmManager.rm.ringMissed();
      return;
    }

    // Debug.Log("Start");
    timeSpaned = 0;

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
      magnitude /= stickPushMaxSpeedAt;
      stickSpeedScale = magnitude > 1 ? 1 : magnitude;
      stopping = false;
    }
    dirIndicator.setDirection(dir);
  }

  public void effectedBy(string objTag, bool effectOn = true) {
    if (usingPowerBoost) return;

    if (objTag == "Metal") {
      unstoppable = effectOn;
    } else if (objTag == "Magnet") {
      usingMagnet = effectOn;
    } else if (objTag == "Monster") {
      ridingMonster = effectOn;
      if (effectOn) minimonCounter = 0;
    } else if (objTag == "EMP") {
      usingEMP = effectOn;
      rb.isKinematic = effectOn;
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
      isRotatingByRainbow = false;
      isRidingRainbowRoad = false;
      rb.isKinematic = false;
      rotatePlayerBody();
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

  public bool isUsingTransformer() {
    return usingTransformer;
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

  public void destroyObject(string tag) {
    numDestroyObstacles++;
    DataManager.dm.increment("TotalNumDestroyObstacles");

    if (tag == "Obstacle_small") {
      DataManager.dm.increment("NumDestroySmallAsteroids");
    }
    else if (tag == "Obstacle_big") {
      DataManager.dm.increment("NumDestroyAsteroids");
    }
    else if (tag == "Obstacle") {
      DataManager.dm.increment("NumDestroyMeteroids");
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
    else Debug.Log("Encounter Exception? " + tag);
  }

  void OnDisable() {
    DataManager.dm.setBestInt("BestBoosters", numBoosters);
    DataManager.dm.setBestInt("BestNumDestroyObstacles", numDestroyObstacles);
    DataManager.dm.setBestInt("BestNumUseObjects", numUseObjects);
  }

  public bool isInvincible() {
    return afterStrengthen || ridingMonster || unstoppable || isRebounding() || isUsingRainbow() || changeManager.isTeleporting();
  }

  public bool canBeMagnetized() {
    return !(isRebounding() || isUsingRainbow() || changeManager.isTeleporting());
  }

  public void resetAbility() {
    baseSpeed = CharacterManager.cm.baseSpeedStandard;
    boosterSpeedUpAmount = CharacterManager.cm.boosterPlusSpeedStandard;
    maxBoosterSpeed = CharacterManager.cm.boosterMaxSpeedStandard;
    boosterSpeedDecreaseBase = CharacterManager.cm.boosterSpeedDecreaseStandard;
    reboundScale = CharacterManager.cm.reboundTimeScaleStandard;
  }

  // public void startBeat() {
  //   GetComponent<Animation>().Play();
  // }

  public void setFever(bool val) {
    feverTime = val;
  }

  public void scaleChange(bool val, float amount) {
    if (!scaleChanged && val) {
      scaleChanged = true;
      transform.localScale *= amount;
      RhythmManager.rm.turnBoostOK(true);
    } else if (scaleChanged && !val && !feverTime) {
      scaleChanged = false;
      transform.localScale /= amount;
      RhythmManager.rm.turnBoostOK(false);
    }
  }

  public void setSpeedBoost(float speedUp, float duration) {
    speedBoosting = true;
    maxSpeedBoostScale = speedUp;
    speedBoostScale = speedUp;
    speedBoostDuration = duration;
    speedBoostCount = 0;
  }

  public void scaleBackByMonster() {
    transform.localScale -= (monm.enlargeScalePerMinimon * minimonCounter) * Vector3.one;
  }
}