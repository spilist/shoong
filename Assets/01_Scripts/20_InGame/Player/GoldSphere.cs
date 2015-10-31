using UnityEngine;
using System.Collections;

public class GoldSphere : MonoBehaviour {
  void OnTriggerEnter(Collider other) {
    if (other.tag == "Part") {
      NormalPartsMover mover = other.GetComponent<NormalPartsMover>();
      mover.transformToGold(transform.position);
    }
  }
}
