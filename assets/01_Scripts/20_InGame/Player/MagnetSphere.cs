using UnityEngine;
using System.Collections;

public class MagnetSphere : MonoBehaviour {
  public MagnetManager mm;

  void OnEnable() {
    transform.localScale = Vector3.one * mm.radiusPerLevel[mm.level];
    GetComponent<ParticleSystem>().startLifetime = mm.lifetimePerLevel[mm.level];
    transform.Find("Halo").GetComponent<Light>().range = mm.radiusPerLevel[mm.level];
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
