using UnityEngine;
using System.Collections;

public class VacuumField : MonoBehaviour {
  VacuumAlienshipManager vam;
  int gravity = 100;
  int gravityToCandies;
  float gravityScale = 10;
  float counter = 0;

  void Start() {
    vam = GameObject.Find("Field Objects").GetComponent<VacuumAlienshipManager>();
    gravity = vam.gravity;
    gravityToCandies = vam.gravityToCandies;
    gravityScale = vam.gravityScale;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") return;
    other.GetComponent<ObjectsMover>().insideBlackhole(gravityToCandies, transform.parent.position - other.transform.position);
  }

  void OnTriggerStay(Collider other) {
    string tag = other.tag;
    if (tag != "Player") return;
    if (Player.pl.cannotBeMagnetized()) return;

    Vector3 heading = transform.parent.position - other.transform.position;
    heading /= heading.magnitude;

    counter += Time.deltaTime;
    Player.pl.rb.AddForce(heading * (gravity + counter * gravityScale), ForceMode.VelocityChange);
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player") counter = 0;
  }
}
