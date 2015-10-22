using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartsToCollectBoundary : MonoBehaviour {
  public Transform inGameUI;
  public GameObject indicatorToCollect;
  public List<GameObject> indicatorPool;
  public int indicatorAmount = 20;

  void Start() {
    indicatorPool = new List<GameObject>();
    for (int i = 0; i < indicatorAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(indicatorToCollect);
      obj.SetActive(false);
      indicatorPool.Add(obj);
    }
  }

  GameObject getIndicator() {
    for (int i = 0; i < indicatorPool.Count; i++) {
      if (!indicatorPool[i].activeInHierarchy) {
        return indicatorPool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(indicatorToCollect);
    obj.transform.parent = transform;
    indicatorPool.Add(obj);
    return obj;
  }

  void OnTriggerEnter(Collider other) {
    string tag = other.tag;
    if (tag == "Part" || tag == "ComboPart" || tag == "SummonedPart") {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      if (mover.hasIndicator()) {
        mover.showIndicator();
      } else {
        GameObject indicator = getIndicator();
        indicator.SetActive(true);
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
