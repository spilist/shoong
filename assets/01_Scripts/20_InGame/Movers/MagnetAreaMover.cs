using UnityEngine;
using System.Collections;

public class MagnetAreaMover : MonoBehaviour {
  public bool pull = false;
  int powerToParts;
  int powerToPlayer;
  int sign;

  void Start() {
    powerToParts = transform.parent.GetComponent<MagnetMover>().powerToParts;
    powerToPlayer = pull ? transform.parent.GetComponent<MagnetMover>().powerToPlayer_pull : transform.parent.GetComponent<MagnetMover>().powerToPlayer_push;
    sign = pull ? 1 : -1;
  }

  void OnEnable() {
    transform.eulerAngles = new Vector3(90, 0, 0);
  }

  void OnTriggerStay(Collider other) {
    string tag = other.tag;
    if (tag == "Player" && !other.GetComponent<PlayerMover>().canBeMagnetized()) return;

    if (tag == "GoldenCube") return;

    int power = tag == "Player" ? powerToPlayer : powerToParts;

    Vector3 heading = transform.parent.position - other.transform.position;
    heading /= heading.magnitude;
    other.GetComponent<Rigidbody>().AddForce(heading * power * sign, ForceMode.VelocityChange);
  }

  // void OnTriggerExit(Collider other) {
  //   string tag = other.tag;

  //   if (tag == "Player") {
  //     other.GetComponent<PlayerMover>().magnetizeEnd();
  //   } else {
  //     other.GetComponent<ObjectsMover>().magnetizeEnd();
  //   }
  // }
}
