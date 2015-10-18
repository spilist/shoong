using UnityEngine;
using System.Collections;

public class TinyForceField_main : MonoBehaviour {
  DoppleManager dpm;
  float dispenserSize = 0;

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
    } else if (dpm.player.isTrapped()) {
      dispenserSize = mover.GetComponent<Collider>().bounds.size.x;
      mover.GetComponent<CubeDispenserMover>().tryBreak();
      return;
    } else {
      instantiateCube(mover);
      mover.destroyObject(true, true);
    }
  }

  void Update() {
    if (dpm.player.isTrapped() && dispenserSize > 0 && GetComponent<Collider>().bounds.size.x > dispenserSize) {
      Destroy(transform.parent.gameObject);
    }
  }

  void instantiateCube(ObjectsMover mover) {
    if (mover.noCubesByDestroy()) return;

    dpm.player.goodPartsEncounter(mover, mover.cubesWhenDestroy(), 0, false);

    if (!mover.isNegativeObject()) {
      dpm.getEnergy.Play();
      dpm.getEnergy.GetComponent<AudioSource>().Play();
    } else {
      DataManager.dm.increment("NumDestroyObstaclesWithBlink");
    }
  }
}
