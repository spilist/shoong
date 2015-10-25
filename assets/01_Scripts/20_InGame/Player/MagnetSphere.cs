using UnityEngine;
using System.Collections;

public class MagnetSphere : MonoBehaviour {
  public Skill_Magnet mm;

  void OnEnable() {
    transform.localScale = Vector3.one * mm.radiusPerLevel[mm.level] * mm.getRadiusScale();
    GetComponent<ParticleSystem>().startLifetime = mm.lifetimePerLevel[mm.level] * mm.getRadiusScale();
    transform.Find("Halo").GetComponent<Light>().range = mm.radiusPerLevel[mm.level] * mm.getRadiusScale();
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
