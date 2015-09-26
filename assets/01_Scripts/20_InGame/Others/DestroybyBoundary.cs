using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
	void Start() {
    transform.localScale = new Vector3(Screen.width * 1.5f, transform.localScale.y, Screen.height * 1.5f);
  }

  void OnTriggerExit(Collider other) {
    ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();
    if (mover == null) Debug.Log(other.tag);
    mover.destroyObject(false);
	}
}
