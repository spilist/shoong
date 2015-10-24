using UnityEngine;
using System.Collections;

public class BlackholeGravitySphere : MonoBehaviour {
  private BlackholeManager blm;
  int gravity;
  int gravityToUser;
  float gravityScale;
  float counter = 0;

  void Start() {
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    gravity = blm.gravity;
    gravityScale = blm.gravityScale;
    gravityToUser = blm.gravityToUser;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") return;
    other.GetComponent<ObjectsMover>().insideBlackhole(gravity, transform.parent.position - other.transform.position);
  }

  void OnTriggerStay(Collider other) {
    string tag = other.tag;
    if (tag != "Player") return;
    if (!Player.pl.canBeMagnetized()) return;

    Vector3 heading = transform.parent.position - other.transform.position;
    heading /= heading.magnitude;

    counter += Time.deltaTime;
    other.GetComponent<Rigidbody>().AddForce(heading * (gravityToUser + counter * gravityScale), ForceMode.VelocityChange);
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player") counter = 0;
  }
}
