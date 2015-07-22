using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public SpecialObjectsManager som;

	void OnTriggerExit(Collider other) {
    if (other.tag == "SpecialPart") {
      Destroy(other.gameObject.GetComponent<GenerateNextSpecial>().getNext());
      Destroy(other.gameObject);
      som.run();
    }
    else {
      Destroy(other.gameObject);
    }
	}

}
