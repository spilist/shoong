using UnityEngine;
using System.Collections;

public class FieldObjectsMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private float unstoppableFollowSpeed;
  private Vector3 direction;

  private bool isMagnetized = false;
  private bool isCollected = false;
  private FieldObjectsManager fom;
  private PartsCollector partsCollector;
  private GameObject player;

  void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    speed = fom.getSpeed(gameObject.tag);
    tumble = fom.getTumble(gameObject.tag);
    unstoppableFollowSpeed = fom.getUnstoppableFollowSpeed();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y);
    direction.Normalize();

    GetComponent<Rigidbody> ().velocity = direction * speed;

    partsCollector = GameObject.Find("PartsCollector").GetComponent<PartsCollector>();

    player = GameObject.Find("Player");
	}

	void FixedUpdate () {
    if (isCollected) {
      Vector3 heading =  partsCollector.transform.position - transform.position;
      heading /= heading.magnitude;

      if (transform.localScale.x > 0.4) {
        transform.localScale -= Vector3.one * 0.05f;
        GetComponent<Rigidbody> ().velocity = heading * 50;

      } else {
        GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.01f;
      }
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

  public void collected() {
    gameObject.tag = "CollectedPart";
    gameObject.layer = LayerMask.NameToLayer("CollectedParts");
    // GetComponent<MeshRenderer>().material = fom.collectedPartsMaterial;
    GetComponent<MeshRenderer>().enabled = false;
    // transform.localScale = Vector3.one * 0.4f;
    StartCoroutine("startCollect");
  }

  IEnumerator startCollect() {
    yield return new WaitForSeconds(1);
    // GetComponent<MeshRenderer>().enabled = true;
    partsCollector.collect();
    Destroy(gameObject);
  }
}
