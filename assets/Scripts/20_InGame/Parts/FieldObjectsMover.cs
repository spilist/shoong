using UnityEngine;
using System.Collections;

public class FieldObjectsMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private float unstoppableFollowSpeed;
  private Vector3 direction;

  private bool isMagnetized = false;
  private FieldObjectsManager fom;
  private PartsCollector partsCollector;
  private GameObject player;

  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;

  void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    shrinkedScale = transform.localScale.x;

    speed = fom.getSpeed(gameObject.tag);
    tumble = fom.getTumble(gameObject.tag);
    unstoppableFollowSpeed = fom.getUnstoppableFollowSpeed();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y);
    direction.Normalize();

    GetComponent<Rigidbody> ().velocity = direction * speed;

    player = GameObject.Find("Player");
	}

	void FixedUpdate () {
    if (isInsideBlackhole) {
      if (blackhole == null) {
        Destroy(gameObject);
        return;
      }
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else if (isMagnetized) {
      Vector3 heading =  player.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * unstoppableFollowSpeed;
    } else {
      GetComponent<Rigidbody> ().velocity = direction * speed;
    }
	}

  void OnCollisionEnter(Collision collision)
  {
    if (isMagnetized) return;

    string colliderTag = collision.collider.tag;

    if (colliderTag == "Part" || colliderTag == "SpecialPart") {
      processCollision(collision);
    }
  }

  void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  public void setMagnetized() {
    isMagnetized = true;
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }
}
