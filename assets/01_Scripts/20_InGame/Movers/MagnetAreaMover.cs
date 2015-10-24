using UnityEngine;
using System.Collections;

public class MagnetAreaMover : MonoBehaviour {
  int power;

  void Start() {
    power = transform.parent.GetComponent<MagnetMover>().power;
  }

  void OnTriggerStay(Collider other) {
    string tag = other.tag;
    if (tag == "Player" && !Player.pl.canBeMagnetized()) return;

    Vector3 heading = other.transform.position - transform.parent.position;
    heading /= heading.magnitude;
    other.GetComponent<Rigidbody>().AddForce(heading * power, ForceMode.VelocityChange);
  }
}
