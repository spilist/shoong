using UnityEngine;
using System.Collections;

public class MagnetSphere : MonoBehaviour {
  public Skill_Magnet mm;

  void OnEnable() {}

  void OnTriggerEnter(Collider other) {
    if (other.GetComponent<ObjectsMover>() != null)
      other.GetComponent<ObjectsMover>().setMagnetized();
    else
      Debug.Log("Magnet try to magentize object that does not have mover: " + other.gameObject.name);
  }
}
