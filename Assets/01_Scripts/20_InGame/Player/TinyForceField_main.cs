using UnityEngine;
using System.Collections;

public class TinyForceField_main : MonoBehaviour {
  DoppleManager dpm;

  void Start () {
    dpm = GameObject.Find("Field Objects").GetComponent<DoppleManager>();
  }

	void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover == null || other.tag == "Blackhole") return;

    dpm.player.goodPartsEncounter(mover, mover.cubesWhenDestroy(), other.tag == "GoldenCube");
  }
}
