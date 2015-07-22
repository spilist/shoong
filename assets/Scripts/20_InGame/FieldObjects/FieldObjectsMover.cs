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

    string tag = gameObject.tag;
    string colliderTag = collision.collider.tag;

    if (tag == "Part") {
      if (colliderTag == "Part" || colliderTag == "SpecialPart") {
        processCollision(collision);
      }
    } else if (tag == "Obstacle") {
      if (colliderTag == "Obstacle") {
        processCollision(collision);
      } else if (colliderTag == "Part") {
        Destroy(collision.collider.gameObject);
      } else if (colliderTag == "SpecialPart") {
        GenerateNextSpecial gns = collision.collider.gameObject.GetComponent<GenerateNextSpecial>();

        if (gns.getComboCount() > 0) {
          // Player was trying to get it
          gns.destroySelf(true, true, false);
        } else {
          // Destroyed somewhere
          gns.destroySelf(true, false, true);
        }
      }
    }
  }

  public void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    Vector3 direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }

  public void setMagnetized() {
    isMagnetized = true;
  }
}
