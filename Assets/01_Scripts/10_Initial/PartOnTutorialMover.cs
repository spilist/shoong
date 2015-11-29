using UnityEngine;
using System.Collections;

public class PartOnTutorialMover : MonoBehaviour {

	void OnEnable () {
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 5;
	}
}
