using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectsMover : MonoBehaviour {
  protected float speed;
  protected float tumble;
  protected Vector3 direction;
  protected bool canBeMagnetized = true;
  protected bool isMagnetized = false;

  protected GameObject player;

  protected BlackholeManager blm;
  protected GameObject blackhole;
  protected bool isInsideBlackhole = false;
  protected float shrinkedScale;

  protected ObjectsManager objectsManager;
  protected Rigidbody rb;

  void Start() {
    player = GameObject.Find("Player");

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

  void FixedUpdate() {
    if (isInsideBlackhole) {
      if (blackhole == null) {
        destroyObject(false);
        return;
      }
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else if (isMagnetized) {
      Vector3 heading =  player.transform.position - transform.position;
      heading /= heading.magnitude;
      rb.velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.5f;
    } else {
      normalMovement();
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (isMagnetized) return;

    ObjectsMover other = collision.collider.gameObject.GetComponent<ObjectsMover>();

    if (other != null) {
      if (strength() == other.strength()) {
        processCollision(collision);
      } else if (strength() > other.strength()) {
        other.destroyObject();
      }
      rb.velocity = direction * speed;
    }

    doSomethingSpecial(collision);
  }

  virtual protected void doSomethingSpecial(Collision collision) {
    return;
  }

  virtual protected float strength() {
    return objectsManager.strength;
  }

  void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  virtual public void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(((BasicObjectsManager)objectsManager).partsDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);
  }

  virtual public void encounterPlayer() {
    if (tag == "Part" && isMagnetized) {
      QuestManager.qm.addCountToQuest("Blackhole");
    }
    Destroy(gameObject);
  }

  public void setMagnetized() {
    if (canBeMagnetized) isMagnetized = true;
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }

  virtual protected float getSpeed() {
    return objectsManager.getSpeed(tag);
  }

  virtual protected float getTumble() {
    return objectsManager.getTumble(tag);
  }

  virtual protected Vector3 getDirection() {
    Vector2 randomV = Random.insideUnitCircle;
    Vector3 dir = new Vector3(randomV.x, 0, randomV.y);
    return dir.normalized;
  }

  virtual protected void initializeRest() {
  }

  virtual protected void normalMovement() {
  }

  virtual public string getManager() {
    return "BasicObjectsManager";
  }
}
