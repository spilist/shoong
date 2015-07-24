using UnityEngine;
using System.Collections;

public class FieldObjectsMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private float unstoppableFollowSpeed;
  private Vector3 direction;

  private bool isMagnetized = false;

  void Start () {
    FieldObjectsManager fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    speed = fom.getSpeed(gameObject.tag);
    tumble = fom.getTumble(gameObject.tag);
    unstoppableFollowSpeed = fom.getUnstoppableFollowSpeed();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);

    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void FixedUpdate () {
    if (isMagnetized) {
      Vector3 heading =  GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
      GetComponent<Rigidbody> ().velocity = heading / heading.magnitude * GameObject.Find("Player").GetComponent<Rigidbody>().velocity.magnitude * unstoppableFollowSpeed;
    }
    else {
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

  public void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  public void setMagnetized() {
    isMagnetized = true;
  }
}
