using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public PatternPartsManager ppm;

	void OnTriggerExit(Collider other) {
    if (other.tag == "SpecialPart") {
      other.gameObject.GetComponent<GenerateNextSpecial>().destroySelf(false, false, true);
    } else if (other.tag == "PatternPart") {
      Destroy(other.transform.parent.gameObject);
      ppm.run();
    } else {
      Destroy(other.gameObject);
    }
	}

}
