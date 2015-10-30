using UnityEngine;
using System.Collections;

public class TransformerSphere : MonoBehaviour {
  public Skill_Transform tfm;
  private int goldRatio;

	void OnEnable() {
    goldRatio = tfm.goldRatio;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big" || other.tag == "Obstacle_small") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();

      mover.transformed(transform.position, transformResult());
    }
  }

  string transformResult() {
    int random = Random.Range(0, 100);
    string result = "";
    if (random < goldRatio) {
      result = "Golden";
    }
    return result;
  }
}
