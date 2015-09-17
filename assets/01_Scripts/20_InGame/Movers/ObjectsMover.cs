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

  protected BlackholeManager blm;
  protected bool isInsideBlackhole = false;
  protected bool destroyed = false;
  protected float shrinkedScale;

  protected ObjectsManager objectsManager;
  protected Rigidbody rb;

  void Start() {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    objectsManager = (ObjectsManager) GameObject.Find("Field Objects").GetComponent(getManager());

    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();

    shrinkedScale = transform.localScale.x;

    initializeRest();

    speed = getSpeed();
    tumble = getTumble();
    direction = getDirection();

    rb = GetComponent<Rigidbody>();
    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = direction * speed;
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
    if (isInsideBlackhole) {
      if (blm.instance == null) {
        destroyObject(false);
        return;
      }

      rb.velocity = blm.headingToBlackhole(transform) * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else if (isMagnetized) {
      if (player.scoreManager.isGameOver()) return;
      Vector3 heading =  player.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.5f;
    } else {
      normalMovement();
    }
  }

  virtual protected void normalMovement() {}

  virtual protected bool beforeCollide(ObjectsMover other) {
    return true;
  }

  void OnCollisionEnter(Collision collision) {
    if (isMagnetized) return;

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

  virtual public void destroyObject(bool destroyEffect = true) {
    if (!beforeDestroy()) return;

    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    if (destroyEffect && objectsManager.objDestroyEffect != null) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
    }

    afterDestroy();

    objectsManager.runImmediately();
  }

  virtual protected void afterDestroy() {}

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

    if (!isNegativeObject()) {
      player.transform.parent.Find("Bars Canvas").GetComponent<ComboBar>().addCombo();
    }

    afterEncounter();
  }

  virtual protected void afterEncounter() {}

  virtual public void setMagnetized() {
    if (canBeMagnetized) isMagnetized = true;
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
  }

  virtual public int cubesWhenEncounter() {
    return objectsManager.cubesWhenEncounter();
  }

  virtual public int bonusCubes() {
    return 0;
  }

  virtual public void destroyByMonster() {
  }

  virtual public bool dangerous() {
    return false;
  }

  virtual public bool isNegativeObject() {
    return objectsManager.isNegative;
  }
}
