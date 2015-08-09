using UnityEngine;
using System.Collections;

public class UnstoppableSphere : MonoBehaviour {
  void OnTriggerEnter(Collider other) {
    if (other.tag == "Part") {
      other.GetComponent<FieldObjectsMover>().setMagnetized();
    } else if (other.tag == "Monster") {
      other.GetComponent<MonsterMover>().setMagnetized();
    }
  }
}
