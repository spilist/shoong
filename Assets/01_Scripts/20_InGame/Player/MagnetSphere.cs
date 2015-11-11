using UnityEngine;
using System.Collections;

public class MagnetSphere : MonoBehaviour {
  public Skill_Magnet mm;

  void OnEnable() {}

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
