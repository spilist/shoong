using UnityEngine;
using System.Collections;

public class GetBlackhole : MonoBehaviour {
  public int[] radiusPerLevel;
  public float[] lifetimePerLevel;

  void OnEnable() {
    int level = DataManager.dm.getInt("BlackholeLevel") - 1;
    transform.localScale = Vector3.one * radiusPerLevel[level];
    GetComponent<ParticleSystem>().startLifetime = lifetimePerLevel[level];
    transform.Find("Halo").GetComponent<Light>().range = radiusPerLevel[level];
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
