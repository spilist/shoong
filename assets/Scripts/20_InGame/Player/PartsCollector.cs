using UnityEngine;
using System.Collections;

public class PartsCollector : MonoBehaviour {
	public PlayerMover player;
  public float startOffset = 20f;

  private float offset;

  void Start() {
    offset = startOffset;
  }

  void Update() {
    Vector3 heading = new Vector3 (player.transform.position.x - player.getDirection().x * offset - transform.position.x, 0, player.transform.position.z - player.getDirection().z * offset - transform.position.z);

    if (heading.magnitude < 5.0f) {
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude;
    } else {
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.5f;
    }

    transform.rotation = player.transform.rotation;
  }

  void OnTriggerEnter(Collider other) {
    Destroy(other.gameObject);
  }
}
