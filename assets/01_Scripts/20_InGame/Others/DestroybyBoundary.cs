using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
	void OnTriggerExit(Collider other) {
    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();
    if (mover == null) Debug.Log(other.tag);
    mover.destroyObject(false);
	}
}
