using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {

	public Transform cubecenter;

	void Update(){
		transform.position = cubecenter.position;
	}

	void OnTriggerExit(Collider other) {
		// Destroy everything that leaves the trigger
		Destroy(other.gameObject);
	}

}
