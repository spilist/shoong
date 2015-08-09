using UnityEngine;
using System.Collections;

public class ObstaclesMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private Vector3 direction;
  private FieldObjectsManager fom;
  private ComboPartsManager cpm;
  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;

	void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    shrinkedScale = transform.localScale.x;

    ObstaclesManager obm = GameObject.Find("Field Objects").GetComponent<ObstaclesManager>();

    speed = obm.speed;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * obm.tumble;

    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;
    GetComponent<Rigidbody>().velocity = direction * speed;
	}

	void FixedUpdate () {
    if (isInsideBlackhole) {
      if (blackhole == null) Destroy(gameObject);
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

    if (colliderTag == "Obstacle") {
      processCollision(collision);
    } else if (colliderTag == "Part") {
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "SpecialPart") {
      fom.spawnSpecial();
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
