using UnityEngine;
using System.Collections;

public class GetBlackhole : MonoBehaviour {
  public int[] radiusPerLevel;

  void OnEnable() {
    transform.localScale = Vector3.one * radiusPerLevel[DataManager.dm.getInt("BlackholeLevel") - 1];
  }

  void OnTriggerEnter(Collider other) {
    other.GetComponent<ObjectsMover>().setMagnetized();
  }
}
