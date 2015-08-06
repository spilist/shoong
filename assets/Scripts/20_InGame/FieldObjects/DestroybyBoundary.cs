using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public FieldObjectsManager fom;
  public ComboPartsManager cpm;

	void OnTriggerExit(Collider other) {
    if (other.tag == "SpecialPart") {
      fom.spawn(fom.special_single);
      Destroy(other.gameObject);
    } else if (other.tag == "ComboPart") {
      cpm.destroyInstances();
    } else {
      Destroy(other.gameObject);
    }
	}
}
