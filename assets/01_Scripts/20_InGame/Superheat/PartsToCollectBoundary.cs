using UnityEngine;
using System.Collections;

public class PartsToCollectBoundary : MonoBehaviour {
  public Transform inGameUI;
  public GameObject indicatorToCollect;

  void OnTriggerEnter(Collider other) {
    string tag = other.tag;
    if (tag == "Part" || tag == "ComboPart" || tag == "SummonedPart") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      if (mover.hasIndicator()) {
        mover.showIndicator();
      } else {
        GameObject indicator = (GameObject)Instantiate(indicatorToCollect);
        mover.setIndicator(indicator);
      }
    }
  }

  void OnTriggerExit(Collider other) {
    string tag = other.tag;
    if (tag == "Part" || tag == "ComboPart" || tag == "SummonedPart") {
      other.GetComponent<ObjectsMover>().hideIndicator();
    }
  }
}
