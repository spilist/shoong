using UnityEngine;
using System.Collections;

public class OffsetFixer : MonoBehaviour {
  GameObject parent;

	void Update () {
    if (parent != null) {
      GetComponent<Rigidbody>().velocity = parent.GetComponent<Rigidbody>().velocity;
      GetComponent<Rigidbody>().angularVelocity = parent.GetComponent<Rigidbody>().angularVelocity;
    }
	}

  public void setParent(GameObject target) {
    parent = target;
  }
}
