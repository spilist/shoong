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
  protected Rigidbody rb;
  public float boundingSize = 50;
  protected Vector3 headingToBlackhole;

  protected bool isTransforming = false;
  protected float transformDuration;
  protected string transformResult;
  protected GameObject transformParticle;
  protected int transformLevel;
  protected GameObject indicator;

  void Start() {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    objectsManager = (ObjectsManager) GameObject.Find("Field Objects").GetComponent(getManager());
    spm = objectsManager.GetComponent<SpecialPartsManager>();
    shrinkedScale = transform.localScale.x;

    initializeRest();

    speed = getSpeed();
    tumble = getTumble();
    direction = getDirection();

    rb = GetComponent<Rigidbody>();
    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = direction * speed;

    originalScale = transform.localScale.x;
  }

  public bool hasIndicator() {
    return indicator != null;
  }

  public void setIndicator(GameObject indicator) {
    this.indicator = indicator;
    indicator.GetComponent<PartsToCollectIndicator>().run(transform, GetComponent<MeshFilter>().sharedMesh);
  }

  public void showIndicator() {
    indicator.SetActive(true);
  }

  public void hideIndicator() {
    indicator.SetActive(false);
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
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime * originalScale / transformDuration);
      transform.localScale = shrinkedScale * Vector3.one;
      if (shrinkedScale == 0) {
        GameObject trParticle = (GameObject) Instantiate(transformParticle, transform.position, Quaternion.identity);
        if (transformResult == "") {
          NormalPartsManager npm = objectsManager.GetComponent<NormalPartsManager>();
          GameObject prefab = npm.partsPrefab[Random.Range(0, npm.partsPrefab.Length)];
          Instantiate(prefab, transform.position, Quaternion.identity);
          trParticle.transform.Find("Normal").gameObject.SetActive(true);
        } else {
          objectsManager.GetComponent<SpawnManager>().runManagerAt(transformResult, transform.position, transformLevel);
          trParticle.transform.Find("Better").gameObject.SetActive(true);
        }
        destroyObject(false);
      }
    }
    else {
      normalMovement();
    }
  }

  public void transformed(Vector3 startPos, GameObject transformLaser, float laserDuration, float duration, GameObject transformParticle, string what, int level) {

    GameObject laser = (GameObject) Instantiate(transformLaser, startPos, Quaternion.identity);
    laser.GetComponent<TransformLaser>().shoot(transform.position, laserDuration);

    StartCoroutine(startTransform(laserDuration, duration, transformParticle, what, level));
  }

  IEnumerator startTransform(float laserDuration, float duration, GameObject transformParticle, string what, int level) {
    yield return new WaitForSeconds(laserDuration);

    isTransforming = true;
    transformDuration = duration;
    transformResult = what;
    this.transformParticle = transformParticle;
    transformLevel = level;
  }

  virtual protected void normalMovement() {}

  virtual protected bool beforeCollide(ObjectsMover other) {
    return true;
  }

  void OnCollisionEnter(Collision collision) {
    // if (isMagnetized) return;

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
    Destroy(gameObject);

    if (destroyEffect && objectsManager.objDestroyEffect != null) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
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

  virtual public void encounterPlayer(bool destroy = true) {
    if (!beforeEncounter()) return;

    if (destroy) {
      foreach (Collider collider in GetComponents<Collider>()) {
        collider.enabled = false;
      }
      Destroy(gameObject);
    }

    if (objectsManager.objEncounterEffect != null) {
      Instantiate(objectsManager.objEncounterEffect, transform.position, transform.rotation);
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

  virtual public void destroyByMonster() {
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

  void OnDestroy() {
    if (indicator != null) Destroy(indicator);
  }
}
