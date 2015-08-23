using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public ComboPartsManager cpm;
  public BlackholeManager blm;
  public CubeDispenserManager cdm;

	void OnTriggerExit(Collider other) {
    if (other.tag == "Blackhole") {
      blm.skipRespawnInterval();
      Destroy(other.gameObject);
    } else {
      ObjectsMover mover = other.gameObject.GetComponent<ObjectsMover>();
      mover.destroyObject();
    }
	}
}
