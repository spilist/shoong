﻿using UnityEngine;
using System.Collections;

public class DestroybyBoundary : MonoBehaviour {
  public SpecialObjectsManager som;

	void OnTriggerExit(Collider other) {
    if (other.tag == "SpecialPart") {
      other.gameObject.GetComponent<GenerateNextSpecial>().destroySelf(false, false, true);
    } else if (other.tag == "CollectedPart") {
    } else {
      Destroy(other.gameObject);
    }
	}

}
