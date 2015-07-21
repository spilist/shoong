using UnityEngine;
using System.Collections;

public class UnstoppableSphere : MonoBehaviour {
  void OnTriggerEnter(Collider other) {
    if (other.tag == "Part") {
      other.GetComponent<FieldObjectsMover>().setMagnetized();
    }
  }
}
