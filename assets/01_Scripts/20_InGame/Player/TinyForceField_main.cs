using UnityEngine;
using System.Collections;

public class TinyForceField_main : MonoBehaviour {
  DoppleManager dpm;

  void Start () {
    dpm = GameObject.Find("Field Objects").GetComponent<DoppleManager>();
  }

	void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (dpm.player.isRidingMonster()) {
      if (mover.tag == "MiniMonster") {
        if (!dpm.player.absorbMinimon(mover)) return;
        instantiateCube(mover);
        QuestManager.qm.addCountToQuest("GetMiniMonster");
      } else {
        dpm.player.generateMinimon(mover);
      }
    }
    else {
      instantiateCube(mover);
    }
  }

  void instantiateCube(ObjectsMover mover) {
    for (int e = 0; e < mover.cubesWhenEncounter(); e++) {
      GameObject cube = (GameObject) Instantiate(dpm.energyCube, mover.transform.position, mover.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(mover.cubesWhenEncounter());
        dpm.player.addCubeCount(mover.cubesWhenEncounter());
      }
    }

    if (!mover.isNegativeObject()) {
      dpm.comboBar.addCombo();
    } else {
      // if (mover.tag == "Obstacle_big") {
      //   QuestManager.qm.addCountToQuest("DestroyAsteroidAndFallingStarByEMP");
      // } else if (mover.tag == "Monster") {
      //   QuestManager.qm.addCountToQuest("DestroyMonsterByEMP");
      // }
    }

    mover.destroyObject();
  }
}
