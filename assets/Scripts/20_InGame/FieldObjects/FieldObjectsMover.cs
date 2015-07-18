using UnityEngine;
using System.Collections;

public class FieldObjectsMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private Vector3 direction;

  void Start () {
    speed = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>().getSpeed(gameObject.tag);
    tumble = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>().getTumble(gameObject.tag);

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    direction = new Vector3(randomV.x, 0, randomV.y);

    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void FixedUpdate () {
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

  void OnCollisionEnter(Collision collision)
  {
    if (gameObject.tag == "Part") {
      if (collision.collider.tag == "Part") {
        processCollision(collision);
      } else if (collision.collider.tag == "SpecialPart") {
        processCollision(collision);
      }
    } else if (gameObject.tag == "Obstacle") {
      if (collision.collider.tag == "Obstacle") {
        processCollision(collision);
      } else if (collision.collider.tag == "Part") {
        Destroy(collision.collider.gameObject);
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
}
