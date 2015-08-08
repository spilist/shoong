using UnityEngine;
using System.Collections;

public class BigObstaclesMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private Vector3 direction;

  private ComboPartsManager cpm;
  private FieldObjectsManager fom;
  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;

  void Start () {
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    shrinkedScale = transform.localScale.x;

    speed = fom.getSpeed(gameObject.tag);
    tumble = fom.getTumble(gameObject.tag);
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y);
    direction.Normalize();
    GetComponent<Rigidbody> ().velocity = direction * speed;
	}

	void FixedUpdate () {
    if (isInsideBlackhole && blackhole != null) {
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else {
      GetComponent<Rigidbody> ().velocity = direction * speed;
    }
	}

  void OnCollisionEnter(Collision collision) {
    string colliderTag = collision.collider.tag;

    if (colliderTag == "Part" || colliderTag == "Obstacle") {
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "SpecialPart") {
      fom.spawnSpecial();
    } else if (colliderTag == "Obstacle_big") {
      processCollision(collision);
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

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }
}
