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
    if (mover.tag == "Blackhole") return;

    if (dpm.player.isRidingMonster()) {
      if (mover.tag == "MiniMonster") {
        if (!dpm.player.absorbMinimon(mover)) return;
        instantiateCube(mover);
        QuestManager.qm.addCountToQuest("GetMiniMonster");
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

    for (int e = 0; e < mover.cubesWhenEncounter(); e++) {
      GameObject cube = (GameObject) Instantiate(dpm.energyCube, mover.transform.position, mover.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(mover.cubesWhenEncounter());
        dpm.player.addCubeCount(mover.cubesWhenEncounter());
      }
    }

    if (!mover.isNegativeObject()) {
      dpm.comboBar.addCombo();
      dpm.getEnergy.Play();
      dpm.getEnergy.GetComponent<AudioSource>().Play();
    } else {
      if (mover.tag == "Obstacle_big" || mover.tag == "Obstacle_small") {
        QuestManager.qm.addCountToQuest("DestroyAsteroid");
      } else if (mover.tag == "Monster") {
        QuestManager.qm.addCountToQuest("DestroyMonster");
      } else if (mover.tag == "Obstacle") {
        QuestManager.qm.addCountToQuest("DestroyFallingStar");
      }
    }
  }
}
