using UnityEngine;
using System.Collections;

public class UnstoppableSphere : MonoBehaviour {
  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
