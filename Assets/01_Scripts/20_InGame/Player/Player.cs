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

  public float monsterDestroyAfter;

  public float baseSpeed;
  public float speed;
  private float stoppingSpeedBuffer = -1;
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

  public float playerSpeedModifierMin = 0.7f;
  public float playerSpeedModifierMax = 1.0f;
  public float lrSpeedModifier = 1.0f;

  public float tumble = 4;
  private Vector3 direction;

  public MonsterManager monm;
  public RainbowDonutsManager rdm;
  public DoppleManager dpm;
  public BlackholeManager blm;
  public IceDebrisManager icm;
  private float icedDuration;
  private float icedSpeedFactor;
  public EnergizerManager egm;
  private float energizedDuration;
  private float energizedReduceDuration;
  private float energizedSpeedFactor;
  public float feveredSpeedFactor;

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
  private bool energized = false;
  private bool usingSolar = false;
  private bool usingGhost = false;
  private bool confused = false;
  [HideInInspector] public bool skill_fevered = false; // For incresing max speed
  public float originalScale;
  private int minimonCounter = 0;

  private bool isRotatingByRainbow = false;
  private bool isRidingRainbowRoad = false;
  private Vector3 rainbowPosition;
  public Rigidbody rb;

  public float afterStrengthenDuration = 1;
  private bool afterStrengthen = false;
  private float afterStrengthenCount = 0;

  public float afterDashDuration = 0.2f;
  private bool afterDash = false;
  private float afterDashCount = 0;

  private CharacterChangeManager changeManager;

  private bool usingPowerBoost = false;

  public int numBoosters = 0;
  private int numDestroyObstacles = 0;
  public PlayerDirectionIndicator dirIndicator;
  private float timeSpaned;
  public ParticleSystem getEnergy;
  public GetPartsText getPartsText;
  public UseBoosterText useBoosterText;
  public Transform contactCollider;
  public int bestX;
  private bool poppingByBooster = false;
  public float poppingSmallScale = 0.6f;
  public float poppingDurationDown = 0.2f;
  public float poppingDurationUp = 0.1f;
  private float poppingDuration;
  private float poppingScale = 1;
  private float poppingTarget;

  public GameObject bonusFilter;

  private bool dashing = false;
  public bool paused = false;
  public PopOnEnable shield;

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

    rb = transform.parent.GetComponent<Rigidbody>();
    rb.velocity = direction * speed;

    transform.parent.localEulerAngles = new Vector3(0, -ContAngle(Vector3.forward, direction), 0);

    if (DataManager.dm.isBonusStage) bonusFilter.SetActive(true);
	}

	void FixedUpdate () {
    // Store speed to buffer when stopping is set to true. If stopping is set to false, set to -1 to indicate it is not stopping
    if (stopping && stoppingSpeedBuffer < 0) {
      stoppingSpeedBuffer = speed;
    } else if (!stopping && stoppingSpeedBuffer >= 0) {
      stoppingSpeedBuffer = -1;
    }

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
        stoppingSpeedBuffer = Mathf.MoveTowards(stoppingSpeedBuffer, 0, Time.fixedDeltaTime * stoppingSpeed);
        speed = stoppingSpeedBuffer;
      } else if (dashing) {
        speed = baseSpeed + boosterspeed + DashManager.dm.speedup;
      } else if (ridingMonster) {
        speed = baseSpeed + minimonCounter * monm.enlargeSpeedPerMinimon + boosterspeed;
      } else if (usingGhost) {
        speed = baseSpeed * 1.6f + boosterspeed;
      } else if (usingSolar) {
        speed = (baseSpeed + boosterspeed) * 1.6f;
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

    if (!uncontrollable() && !bouncing) {
      if (iced) speed *= icedSpeedFactor;
      if (energized) speed *= energizedSpeedFactor;
      if (skill_fevered) speed *= feveredSpeedFactor;
      if (OverHeatManager.ohm.onOverHeat) speed *= OverHeatManager.ohm.characterSpeedIncrease;
    }

    if (boosterspeed > 0) {
      timeSpaned += Time.fixedDeltaTime;
      boosterspeed -= 80 / boosterSpeedDecreaseBase + boosterSpeedDecreasePerTime * Time.fixedDeltaTime;
    } else if (boosterspeed <= 0){
      boosterspeed = 0;
    }

    speed *= lrSpeedModifier;

    rb.velocity = direction * speed * stickSpeedScale;

    if (poppingByBooster) {
      poppingScale = Mathf.MoveTowards(poppingScale, poppingTarget, Time.deltaTime * (1 - poppingSmallScale) / poppingDuration);

      transform.parent.localScale = transform.parent.localScale.x * new Vector3(1, 1, poppingScale);

      if (poppingScale == poppingSmallScale) {
        poppingTarget = 1;
        poppingDuration = poppingDurationUp;
      }

      if (poppingScale == 1) {
        poppingByBooster = false;
      }
    }

    if (dashing) {
      transform.parent.Rotate(0, 0, Time.fixedDeltaTime * 360 * DashManager.dm.numRotateWhenSmash / DashManager.dm.duration);
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

    if (ridingMonster && tag != "MiniMonster" && tag != "RainbowDonut" && tag != "GoldenCube") {
      generateMinimon(mover);
      return;
    }

    if (tag == "IceDebris" || tag == "PhaseMonster" || tag == "BlackMonster" || tag == "ConfuseMonster") {
      goodPartsEncounter(mover, mover.cubesWhenEncounter(), false);
      return;
    }

    if (tag == "MiniMonster") {
      if (!absorbMinimon(mover)) return;
    }

    if (tag == "Blackhole" && !usingSolar) return;

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
  }

  public void goodPartsEncounter(ObjectsMover mover, int howMany, bool encounterPlayer = true) {
    if (mover.tag != "GoldenCube" && howMany > 0) {
      CubeManager.cm.addPoints(howMany, mover.transform.position);
    }

    if (mover.energyGets() > 0) {
      EnergyManager.em.getEnergy(mover.energyGets());
      DashManager.dm.getLarger();
    }

    if (encounterPlayer || (!encounterPlayer && mover.tag == "GoldenCube")) mover.encounterPlayer();
    else mover.destroyObject(true, true);
  }

  public void contactCubeDispenser(Transform tr, int howMany, Collision collision, float reboundDuring) {
    CubeManager.cm.addPoints(howMany, tr.position);
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

    if (amount > 0) {
      changeManager.changeRed();
      EnergyManager.em.loseEnergy(amount, tag);
    }
  }

  public void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    Vector3 dir = Vector3.Reflect(direction, -normal).normalized;
    dir.y = 0;
    dir.Normalize();
    setDirection(dir);
  }

  public Vector3 getDirection() {
    return direction;
  }

  public void teleport() {
    teleport(transform.position + direction * dpm.blinkDistance);
  }

  public void teleport(Vector3 pos) {
    if (changeManager.isTeleporting() || ScoreManager.sm.isGameOver()) return;
    changeManager.teleport(pos);
  }

  public void stopMoving(bool val = true) {
    stopping = val;
    stickSpeedScale = 1;
  }

  public void shootBooster() {
    if (stopping || uncontrollable()) return;

    numBoosters++;

    timeSpaned = 0;

    if (!usingPowerBoost) {
      changeManager.booster.Play();
      changeManager.booster.GetComponent<AudioSource>().Play();
      poppingByBooster = true;
      poppingDuration = poppingDurationDown;

      poppingScale = 1;
      poppingTarget = poppingSmallScale;
    }

    addSpeedByBooster();
  }

  void addSpeedByBooster() {
    if (boosterspeed < maxBooster()) {
      boosterspeed += boosterSpeedUp() * boosterBonus;
      boosterspeed = boosterspeed > maxBooster() ? maxBooster() : boosterspeed;
    }
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
      if (!iced)
        StartCoroutine(iceDebrisSoundEffect());
      iced = effectOn;
      icedDuration = icm.speedRestoreDuring;
      icedSpeedFactor = icm.playerSpeedReduceTo;
    } else if (objTag == "Energizer") {
      if (!energized)
        StartCoroutine(energizedSoundEffect());
      energized = effectOn;
      energizedDuration = egm.speedRestoreStartAfter + egm.speedRestoreDuration;
      energizedReduceDuration = egm.speedRestoreDuration;
      energizedSpeedFactor = egm.playerSpeedIncreaseTo;
    } else if (objTag == "Solar") {
      usingSolar = effectOn;
    } else if (objTag == "Ghost") {
      usingGhost = effectOn;
    }
  }

  IEnumerator iceDebrisSoundEffect() {
    yield return null;
    float moveAmount = AudioManager.am.main.movePitchToPercent(0.8f, 0f);
    Debug.Log("MoveAmount: " + moveAmount);
    while (iced == true) {
        yield return null;
    }
    AudioManager.am.main.movePitch(-moveAmount, 0.5f);
  }

  IEnumerator energizedSoundEffect() {
    yield return null;
    float moveAmount = AudioManager.am.main.movePitchToPercent(1.5f, 0f);
    Debug.Log("MoveAmount: " + moveAmount);
    while (energized == true) {
      yield return null;
    }
    AudioManager.am.main.movePitch(-moveAmount, 0.5f);
  }

  public void stopOtherEffects() {
    iced = false;
    icedDuration = 0;
    icedSpeedFactor = 1;
    icm.strengthenPlayerEffect.SetActive(false);

    energized = false;
    energizedDuration = 0;
    energizedSpeedFactor = 1;
    egm.strengthenPlayerEffect.SetActive(false);

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
    }
    Camera.main.GetComponent<CameraMover>().shakeUntilStop(blm.shakeAmount);
    processCollision(collision);
    reboundingByBlackhole = true;
    bounceDuration = blm.reboundDuring;
  }

  public void afterDashStart() {
    afterDash = true;
    afterDashCount = 0;
    //changeManager.afterStrengthenEffect.Play();
  }

  public bool isAfterDash() {
    return afterDash;
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

  public bool isUsingSolar() {
    return usingSolar;
  }

  public bool isConfused() {
    return confused;
  }

  public void setConfused(bool val) {
    confused = val;
  }

  void offConfused() {
    confused = false;
  }

  public void stopEMP() {
    if (!usingPowerBoost) {
      rb.isKinematic = false;
    }
    usingEMP = false;
    afterStrengthenStart();
  }

  public bool uncontrollable() {
    return isRebounding() || isUsingRainbow() || usingEMP || bouncing || bouncingByDispenser || dashing;
  }

  void useSkill() {
    if (!RhythmManager.rm.isSkillOK || isRebounding() || isUsingRainbow() || usingEMP) return;

    SkillManager.sm.activate();
    if (SkillManager.sm.isBlink()) {
      teleport(transform.position + direction * dpm.blinkDistance);
    }
  }

  public bool noRhythmRing() {
    return isRebounding() || isUsingRainbow() || usingEMP;
  }

  void Update() {
    if (isRotatingByRainbow) {
      Vector3 dir = rainbowPosition - transform.position;
      transform.parent.Translate(dir * Time.deltaTime / rdm.rotateDuring, Space.World);
      transform.parent.Rotate(0, 0, Time.deltaTime * rdm.rotateAngularSpeed);
      // transform.Rotate(-Vector3.forward * Time.deltaTime * rdm.rotateAngularSpeed, Space.World);
    } else {
      transform.parent.Rotate(0, 0, Time.deltaTime * tumble);
    }

    if (dashing) {
      poppingScale = Mathf.MoveTowards(poppingScale, 1, Time.deltaTime * DashManager.dm.maxEnlargeSize / DashManager.dm.duration);
      transform.parent.localScale = poppingScale * Vector3.one;
      contactCollider.localScale = poppingScale * 25f * Vector3.one;
    }

    if (afterDash) {
      if (afterDashCount < afterDashDuration) {
        afterDashCount += Time.deltaTime;
      } else {
        afterDash = false;
      }
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
    if (energized) {
      if (energizedDuration > 0) {
        energizedDuration -= Time.deltaTime;
        if (energizedDuration < energizedReduceDuration) {
          energizedSpeedFactor = Mathf.MoveTowards(energizedSpeedFactor, 1, Time.deltaTime * (1 - egm.playerSpeedIncreaseTo) / egm.speedRestoreDuration);
        }
      } else {
        energized = false;
        egm.strengthenPlayerEffect.SetActive(false);
        egm.run();
      }
    }
  }

  public void destroyObject(string tag) {
    if (!usingEMP) Camera.main.GetComponent<CameraMover>().shake();
  }

  public bool isInvincible() {
    return afterDash || afterStrengthen || ridingMonster || unstoppable || isRebounding() || isUsingRainbow() || changeManager.isTeleporting() || usingEMP || usingSolar || dashing;
  }

  public bool cannotBeMagnetized() {
    return isRebounding() || isUsingRainbow() || changeManager.isTeleporting();
  }

  public void resetAbility(int bestX) {
    baseSpeed = CharacterManager.cm.baseSpeedStandard;
    boosterSpeedUpAmount = CharacterManager.cm.boosterPlusSpeedStandard;
    maxBoosterSpeed = CharacterManager.cm.boosterMaxSpeedStandard;
    boosterSpeedDecreaseBase = CharacterManager.cm.boosterSpeedDecreaseStandard;
    reboundScale = CharacterManager.cm.reboundTimeScaleStandard;

    this.bestX = bestX;
    transform.localEulerAngles = new Vector3(bestX, 0, 0);
  }

  public void setSpeedBoost(float speedUp, float duration) {
    speedBoosting = true;
    maxSpeedBoostScale = speedUp;
    speedBoostScale = speedUp;
    speedBoostDuration = duration;
    speedBoostCount = 0;
  }

  public void scaleChange(float amount) {
    transform.parent.localScale = (1 + amount) * new Vector3(1, 1, transform.localScale.z / transform.localScale.x);
    contactCollider.localScale = transform.parent.localScale.x * 25f * Vector3.one;
    if (shield.gameObject.activeSelf) {
      shield.transform.localScale = transform.parent.localScale.x * shield.scaleStandard * Vector3.one;
    }
  }

  public void scaleBack() {
    transform.localScale = originalScale * Vector3.one;
  }

  float ContAngle(Vector3 fwd, Vector3 targetDir) {
    float angle = Vector3.Angle(fwd, targetDir);

    if (AngleDir(fwd, targetDir, Vector3.up) == -1) {
        angle = 360.0f - angle;
        if( angle > 359.9999f ) angle -= 360.0f;
    }

    if (angle > 180) angle -= 360.0f;
    else if (angle < -180) angle += 360.0f;

    angle = -angle;

    return angle;
  }

  int AngleDir( Vector3 fwd, Vector3 targetDir, Vector3 up) {
    Vector3 perp = Vector3.Cross(fwd, targetDir);
    float dir = Vector3.Dot(perp, up);

    if (dir > 0.0) return 1;
    else if (dir < 0.0) return -1;
    else return 0;
  }

  public void dash() {
    dashing = true;
    poppingScale = DashManager.dm.maxEnlargeSize + 1;
    Invoke("turnDashOff", DashManager.dm.duration);
  }

  public bool isDashing() {
    return dashing || afterDash;
  }

  void turnDashOff() {
    dashing = false;
    addSpeedByBooster();
    Player.pl.afterDashStart();
    DashManager.dm.resetStep();
  }
}
