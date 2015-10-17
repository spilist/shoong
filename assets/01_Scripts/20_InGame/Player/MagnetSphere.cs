using UnityEngine;
using System.Collections;

public class MagnetSphere : MonoBehaviour {
  public MagnetManager mm;

  void OnEnable() {
    int level = DataManager.dm.getInt("MagnetLevel") - 1;

    transform.localScale = Vector3.one * mm.radiusPerLevel[level];
    GetComponent<ParticleSystem>().startLifetime = mm.lifetimePerLevel[level];
    transform.Find("Halo").GetComponent<Light>().range = mm.radiusPerLevel[level];
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
