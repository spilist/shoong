using UnityEngine;
using System.Collections;

public class ObstaclesMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private Vector3 direction;
  private FieldObjectsManager fom;
  private ComboPartsManager cpm;

	void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();

    ObstaclesManager obm = GameObject.Find("Field Objects").GetComponent<ObstaclesManager>();

    speed = obm.speed;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * obm.tumble;

    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;
    GetComponent<Rigidbody>().velocity = direction * speed;
	}

	void FixedUpdate () {
    GetComponent<Rigidbody>().velocity = direction * speed;
	}

  void OnCollisionEnter(Collision collision) {
    string colliderTag = collision.collider.tag;

    if (colliderTag == "Obstacle") {
      processCollision(collision);
    } else if (colliderTag == "Part") {
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "SpecialPart") {
      fom.spawn(fom.special_single);
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "ComboPart") {
      cpm.destroyInstances();
    }
  }

  void processCollision(Collision collision) {
    ContactPoint contact = collision.contacts[0];
    Vector3 normal = contact.normal;
    direction = Vector3.Reflect(direction, -normal).normalized;
    direction.y = 0;
    direction.Normalize();
  }
}
