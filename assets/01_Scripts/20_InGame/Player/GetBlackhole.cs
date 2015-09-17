using UnityEngine;
using System.Collections;

public class GetBlackhole : MonoBehaviour {
  public BlackholeManager blm;

  void OnEnable() {
    int level = DataManager.dm.getInt("BlackholeLevel") - 1;
    transform.localScale = Vector3.one * blm.radiusPerLevel[level];
    GetComponent<ParticleSystem>().startLifetime = blm.lifetimePerLevel[level];
    transform.Find("Halo").GetComponent<Light>().range = blm.radiusPerLevel[level];
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
