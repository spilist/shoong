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
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover != null) other.GetComponent<ObjectsMover>().insideBlackhole(gravity, transform.parent.position - other.transform.position);
  }

  void OnTriggerStay(Collider other) {
    string tag = other.tag;
    if (tag != "Player") return;
    if (Player.pl.cannotBeMagnetized()) return;

    Vector3 heading = transform.parent.position - other.transform.position;
    heading /= heading.magnitude;

    counter += Time.deltaTime;
    Player.pl.rb.AddForce(heading * (gravityToUser + counter * gravityScale), ForceMode.VelocityChange);
    Camera.main.GetComponent<CameraMover>().shake(1, ((counter < 2) ? counter * 2 : 4));
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player") counter = 0;
    Camera.main.GetComponent<CameraMover>().stopShake();
  }
}
