using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectsMover : MonoBehaviour {
  protected float speed;
  protected float tumble;
  protected Vector3 direction;
  protected bool canBeMagnetized = true;
  protected bool isMagnetized = false;

  protected PlayerMover player;
  protected int gravity;
  protected float originalScale;

  protected bool isInsideBlackhole = false;
  protected bool destroyed = false;
  protected float shrinkedScale;

  protected ObjectsManager objectsManager;
  protected SpecialPartsManager spm;
  protected TransformerManager tfm;
  protected Rigidbody rb;
  public float boundingSize = 50;
  protected Vector3 headingToBlackhole;

  protected bool isTransforming = false;
  protected float transformDuration;
  protected string transformResult;
  protected GameObject transformParticle;
  protected int transformLevel;
  protected GameObject indicator;

  void Awake() {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    objectsManager = (ObjectsManager) GameObject.Find("Field Objects").GetComponent(getManager());
    spm = objectsManager.GetComponent<SpecialPartsManager>();
    tfm = objectsManager.GetComponent<TransformerManager>();

    shrinkedScale = transform.localScale.x;
    rb = GetComponent<Rigidbody>();
    originalScale = transform.localScale.x;
    initializeRest();
  }

  void OnEnable() {
    speed = getSpeed();
    tumble = getTumble();
    direction = getDirection();

    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = direction * speed;
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
      if (player.scoreManager.isGameOver()) return;
      Vector3 heading =  player.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.velocity = heading * player.getSpeed() * 1.5f;
    } else if (isInsideBlackhole) {
      rb.velocity = headingToBlackhole * gravity;
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime * originalScale);
      transform.localScale = shrinkedScale * Vector3.one;
      if (shrinkedScale == 0) destroyObject(false);
    } else if (isTransforming) {
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime * originalScale / tfm.transformDuration);
      transform.localScale = shrinkedScale * Vector3.one;
      if (shrinkedScale == 0) {
        GameObject trParticle = tfm.getParticle(transform.position);
        trParticle.SetActive(true);

        if (transformResult == "") {
          objectsManager.GetComponent<NormalPartsManager>().spawnNormal(transform.position);
          trParticle.transform.Find("Normal").gameObject.SetActive(true);
          trParticle.transform.Find("Better").gameObject.SetActive(false);
        } else {
          objectsManager.GetComponent<SpawnManager>().runManagerAt(transformResult, transform.position, transformLevel);
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

  public void transformed(Vector3 startPos, string what, int level) {

    GameObject laser = tfm.getLaser(startPos);
    laser.SetActive(true);
    laser.GetComponent<TransformLaser>().shoot(transform.position, tfm.laserShootDuration);

    StartCoroutine(startTransform(what, level));
  }

  IEnumerator startTransform(string what, int level) {
    yield return new WaitForSeconds(tfm.laserShootDuration);

    isTransforming = true;
    transformResult = what;
    transformLevel = level;
  }

  virtual protected void normalMovement() {}

  virtual protected bool beforeCollide(ObjectsMover other) {
    return true;
  }

  void OnCollisionEnter(Collision collision) {
    ObjectsMover other = collision.collider.gameObject.GetComponent<ObjectsMover>();

    if (other != null) {
      if (other.tag == "Blackhole") return;

      if (beforeCollide(other)) {
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

  void processCollision(Collision collision) {
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

    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }

    gameObject.SetActive(false);

    if (destroyEffect && objectsManager.objDestroyEffect != null) {
      showDestroyEffect();
    }

    afterDestroy(byPlayer);

    if (byPlayer) {
      objectsManager.run();
      if (isNegativeObject()) player.destroyObject(tag, gaugeWhenDestroy());
    } else {
      objectsManager.runImmediately();
    }
  }

  virtual protected void afterDestroy(bool byPlayer) {}

  virtual protected bool beforeEncounter() {
    return true;
  }

  virtual public void showDestroyEffect() {
    GameObject obj = objectsManager.getPooledObj(objectsManager.objDestroyEffectPool, objectsManager.objDestroyEffect, transform.position);
    obj.SetActive(true);
  }

  virtual public void encounterPlayer(bool destroy = true) {
    if (!beforeEncounter()) return;

    if (destroy) {
      foreach (Collider collider in GetComponents<Collider>()) {
        collider.enabled = false;
      }
      gameObject.SetActive(false);
    }

    if (objectsManager.objEncounterEffect != null) {
      showEncounterEffect();
    }

    if (objectsManager.objEncounterEffectForPlayer != null) {
      objectsManager.objEncounterEffectForPlayer.Play();
      if (objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>() != null) {
        objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();
      }
    }

    if (objectsManager.strengthenPlayerEffect != null) {
      objectsManager.strengthenPlayerEffect.SetActive(true);
      player.strengthenBy(tag);
    }

    if (hasEncounterEffect()) {
      player.encounterObject(tag);
    } else if (isNegativeObject()) {
      player.destroyObject(tag, gaugeWhenDestroy());
    }

    afterEncounter();
  }

  virtual public void showEncounterEffect() {
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
    if (isNegativeObject() && player.isUnstoppable()) return (int) (cubesWhenEncounter() * spm.bonus);
    else return 0;
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

  public int gaugeWhenDestroy() {
    return objectsManager.gaugeWhenDestroy;
  }

  void OnDisable() {
    isMagnetized = false;
    transform.localScale = originalScale * Vector3.one;
    shrinkedScale = originalScale;
    isInsideBlackhole = false;
    destroyed = false;
    isTransforming = false;

    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = true;
    }

    if (indicator != null) {
      indicator.SetActive(false);
      indicator = null;
    }
  }
}
