using UnityEngine;
using System.Collections;

public class FlyingBoundary : MonoBehaviour {
	public FlyingCharacters flyingCharacter;

	void OnTriggerEnter(Collider other) {
    // Destroy(other.gameObject);
    // flyingCharacter.showNewCharacter();
  }
}
