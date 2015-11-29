using UnityEngine;
using System.Collections;

public class TransformerSphere : MonoBehaviour {
  public Skill_Transform tfm;
  private int goldRatio;
  private int subRatio;

	void Start() {
    goldRatio = tfm.goldRatio;
    subRatio = tfm.subRatio;
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
    if (random < subRatio) {
      result = tfm.getRandomManagerName();
    } else if (random < subRatio + goldRatio) {
      result = "Golden";
    }
    return result;
  }
}
