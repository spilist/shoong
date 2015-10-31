﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectsMover : MonoBehaviour {
  protected float speed;
  protected float tumble;
  protected Vector3 direction;
  protected bool canBeMagnetized = true;
  protected bool isMagnetized = false;

  protected int gravity;
  protected float originalScale;

  protected bool isInsideBlackhole = false;
  protected bool destroyed = false;
  protected float shrinkedScale;

  protected ObjectsManager objectsManager;
  protected Skill_Transform transSkill;
  protected Rigidbody rb;
  public float boundingSize = 50;
  protected Vector3 headingToBlackhole;

  protected bool isTransforming = false;
  protected float transformDuration;
  protected string transformResult;
  protected GameObject transformParticle;
  protected GameObject indicator;
  public Player player;

  void Awake() {
    objectsManager = (ObjectsManager) GameObject.Find("Field Objects").GetComponent(getManager());

    if (isNegativeObject()) {
      transSkill = (Skill_Transform)SkillManager.sm.getSkill("Transform");
    }

    shrinkedScale = transform.localScale.x;
    rb = GetComponent<Rigidbody>();
    originalScale = transform.localScale.x;
    player = Player.pl;
    initializeRest();
  }

  void OnEnable() {
    speed = getSpeed();
    tumble = getTumble();
    direction = getDirection();

    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = direction * speed;

    destroyed = false;
    isTransforming = false;
    isMagnetized = false;
    isInsideBlackhole = false;
    transformResult = "";

    afterEnable();
  }

  virtual protected void afterEnable() {}

  public bool hasIndicator() {
    return indicator != null;
  }

  public void setIndicator(GameObject indicator) {
    this.indicator = indicator;
    indicator.GetComponent<PartsToCollectIndicator>().run(transform, GetComponent<MeshFilter>().sharedMesh);
  }

  public void showIndicator() {
    indicator.GetComponent<Renderer>().enabled = true;
  }

  public void hideIndicator() {
    indicator.GetComponent<Renderer>().enabled = false;
  }

  public void setBoundingSize(float val) {
    boundingSize = val;
  }

  public float getBoundingSize() {
    return boundingSize;
  }

  virtual public string getManager() {
    return "";
  }

  virtual protected void initializeRest() {}

  virtual protected float strength() {
    return objectsManager.strength;
  }

  virtual protected float getSpeed() {
    return objectsManager.getSpeed();
  }

  virtual protected float getTumble() {
    return objectsManager.getTumble();
  }

  virtual protected Vector3 getDirection() {
    return objectsManager.getDirection();
  }

  void FixedUpdate() {
    if (isMagnetized) {
      if (ScoreManager.sm.isGameOver()) return;
      Vector3 heading =  Player.pl.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.velocity = heading * (Player.pl.baseSpeed + Player.pl.getSpeed());
    } else if (isInsideBlackhole) {
      rb.velocity = headingToBlackhole * gravity;
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime * originalScale);
      transform.localScale = shrinkedScale * Vector3.one;
      if (shrinkedScale == 0) destroyObject(false);
    } else if (isTransforming) {
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime * originalScale / transSkill.transformDuration);
      transform.localScale = shrinkedScale * Vector3.one;
      if (shrinkedScale == 0) {
        GameObject trParticle = transSkill.getParticle(transform.position);
        trParticle.SetActive(true);

        if (transformResult == "") {
          objectsManager.GetComponent<NormalPartsManager>().spawnNormal(transform.position);
          trParticle.transform.Find("Normal").gameObject.SetActive(true);
          trParticle.transform.Find("Better").gameObject.SetActive(false);
        } else {
          objectsManager.GetComponent<GoldenCubeManager>().spawnGoldenCube(transform.position);
          trParticle.transform.Find("Normal").gameObject.SetActive(false);
          trParticle.transform.Find("Better").gameObject.SetActive(true);
        }
        destroyObject(false);
      }
    }
    else {
      normalMovement();
    }
  }

  public void transformed(Vector3 startPos, string what) {

    GameObject laser = transSkill.getLaser(startPos);
    laser.SetActive(true);
    laser.GetComponent<TransformLaser>().shoot(transform.position, transSkill.laserShootDuration);

    StartCoroutine(startTransform(what));
  }

  IEnumerator startTransform(string what) {
    yield return new WaitForSeconds(transSkill.laserShootDuration);

    isTransforming = true;
    transformResult = what;
  }

  virtual protected void normalMovement() {}

  virtual protected bool beforeCollide(Collision collision) {
    if (collision.collider.tag == "ContactCollider" && dangerous()) {
      Player.pl.loseEnergy(objectsManager.loseEnergyWhenEncounter, tag);
      Player.pl.bounce(objectsManager.bounceDuration, collision);

      if (objectsManager.strengthenPlayerEffect != null) {
        objectsManager.strengthenPlayerEffect.SetActive(true);
        Player.pl.effectedBy(tag);
      }

      afterCollidePlayer();
      return false;
    }

    return true;
  }

  virtual protected void afterCollidePlayer() {}

  void OnCollisionEnter(Collision collision) {
    if (beforeCollide(collision)) {
      ObjectsMover other = collision.collider.gameObject.GetComponent<ObjectsMover>();

      if (other != null) {
        if (other.tag == "Blackhole") return;

        if (strength() == other.strength()) {
          processCollision(collision);
        } else if (strength() < other.strength()) {
          destroyObject();
        }
        rb.velocity = direction * speed;
      }
    }
    afterCollide(collision);
  }

  protected void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  virtual protected void afterCollide(Collision collision) {}

  virtual protected bool beforeDestroy() {
    if (destroyed) {
      return false;
    } else {
      destroyed = true;
      return true;
    }
  }

  virtual public void destroyObject(bool destroyEffect = true, bool byPlayer = false) {
    if (!beforeDestroy()) return;

    gameObject.SetActive(false);

    if (destroyEffect) {
      showDestroyEffect(byPlayer);
    }

    afterDestroy(byPlayer);

    if (byPlayer) {
      objectsManager.run();
      if (isNegativeObject()) Player.pl.destroyObject(tag);
    } else {
      objectsManager.runImmediately();
    }
  }

  virtual protected void afterDestroy(bool byPlayer) {}

  virtual protected bool beforeEncounter() {
    return true;
  }

  virtual public void showDestroyEffect(bool byPlayer) {
    if (objectsManager.objDestroyEffect == null) return;

    GameObject obj = objectsManager.getPooledObj(objectsManager.objDestroyEffectPool, objectsManager.objDestroyEffect, transform.position);
    obj.SetActive(true);
  }

  virtual public void encounterPlayer(bool destroy = true) {
    if (!beforeEncounter()) return;

    if (destroy) {
      gameObject.SetActive(false);
    }

    showEncounterEffect();

    if (objectsManager.objEncounterEffectForPlayer != null) {
      objectsManager.objEncounterEffectForPlayer.Play();
      if (objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>() != null) {
        objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();
      }
    }

    if (objectsManager.strengthenPlayerEffect != null) {
      objectsManager.strengthenPlayerEffect.SetActive(true);
      Player.pl.effectedBy(tag);
    }

    if (hasEncounterEffect()) {
      Player.pl.encounterObject(tag);
    } else if (isNegativeObject() && !dangerous()) {
      Player.pl.destroyObject(tag);
    }

    afterEncounter();
  }

  virtual public void showEncounterEffect() {
    if (objectsManager.objEncounterEffect == null) return;

    GameObject obj = objectsManager.getPooledObj(objectsManager.objEncounterEffectPool, objectsManager.objEncounterEffect, transform.position);
    obj.SetActive(true);
  }

  virtual protected void afterEncounter() {}

  virtual public void setMagnetized() {
    if (canBeMagnetized) isMagnetized = true;
  }

  public void insideBlackhole(int gravity, Vector3 heading) {
    isInsideBlackhole = true;
    this.gravity = gravity;
    headingToBlackhole = heading / heading.magnitude;
  }

  virtual public int cubesWhenEncounter() {
    return objectsManager.cubesWhenEncounter();
  }

  virtual public int cubesWhenDestroy() {
    return objectsManager.cubesWhenEncounter();
  }

  virtual public int bonusCubes() {
    return 0;
  }

  virtual public bool dangerous() {
    return false;
  }

  virtual public bool isNegativeObject() {
    return objectsManager.isNegative;
  }

  virtual public bool hasEncounterEffect() {
    return objectsManager.hasEncounterEffect;
  }

  virtual public bool noCubesByDestroy() {
    return false;
  }

  void OnDisable() {
    isMagnetized = false;
    transform.localScale = originalScale * Vector3.one;
    shrinkedScale = originalScale;
    isInsideBlackhole = false;
    destroyed = false;
    isTransforming = false;

    if (indicator != null) {
      indicator.SetActive(false);
      indicator = null;
    }
  }
}
