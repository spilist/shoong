using UnityEngine;
using System.Collections;

public class TransformerArea : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big" || other.tag == "Obstacle_small") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      if (mover!= null) mover.transformed(transform.position, "");
    }
  }
}
