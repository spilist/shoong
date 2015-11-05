using UnityEngine;
using System.Collections;

public class PartOnTutorialBoundary : MonoBehaviour {
  Vector3 position;
	void Awake() {
    position = Player.pl.transform.position;
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player") {
      Player.pl.transform.position = position;
    }
  }
}
