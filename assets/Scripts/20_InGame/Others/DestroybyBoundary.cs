using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public FieldObjectsManager fom;
  public ComboPartsManager cpm;
  public BlackholeManager blm;

	void OnTriggerExit(Collider other) {
    if (other.tag == "SpecialPart") {
      fom.spawnSpecial();
    } else if (other.tag == "ComboPart") {
      cpm.destroyInstances();
    } else if (other.tag == "Blackhole") {
      Debug.Log("destroyed by boundary?");
      blm.skipRespawnInterval();
      Destroy(other.gameObject);
    } else {
      Destroy(other.gameObject);
    }
	}
}
