using UnityEngine;
using System.Collections;

public class TransformerArea : MonoBehaviour {
  TransformerManager tfm;
  private GameObject transformParticle;
  private GameObject transformLaser;
  float transformDuration;
  float laserShootDuration;

	void Start () {
    tfm = GameObject.Find("Field Objects").GetComponent<TransformerManager>();
    transformParticle = tfm.transformParticle;
    transformDuration = tfm.transformDuration;
    transformLaser = tfm.transformLaser;
    laserShootDuration = tfm.laserShootDuration;
	}

	void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big" || other.tag == "Obstacle_small") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      if (mover!= null) StartCoroutine(mover.transformed(transform.position, transformLaser, laserShootDuration, transformDuration, transformParticle, "", 0));
    }
  }
}
