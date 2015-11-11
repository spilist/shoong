using UnityEngine;
using System.Collections;

public class TinyForceField_main : MonoBehaviour {
  DoppleManager dpm;

  void Start () {
    dpm = GameObject.Find("Field Objects").GetComponent<DoppleManager>();
  }

	void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover == null) return;

    if (mover.tag == "Blackhole") return;

    if (dpm.player.isRidingMonster()) {
      if (mover.tag == "MiniMonster") {
        if (!dpm.player.absorbMinimon(mover)) return;
        instantiateCube(mover);
      } else {
        dpm.player.generateMinimon(mover);
      }
    } else {
      instantiateCube(mover);
      mover.destroyObject(true, true);
    }
  }

  void instantiateCube(ObjectsMover mover) {
    if (mover.noCubesByDestroy()) return;

    dpm.player.goodPartsEncounter(mover, mover.cubesWhenDestroy(), false);

    if (!mover.isNegativeObject()) {
      dpm.getEnergy.Play();
      dpm.getEnergy.GetComponent<AudioSource>().Play();
    } else {
      DataManager.dm.increment("NumDestroyObstaclesWithBlink");
    }
  }
}
