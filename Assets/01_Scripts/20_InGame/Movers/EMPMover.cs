using UnityEngine;
using System.Collections;

public class EMPMover : ObjectsMover {
  EMPManager empManager;

	override protected void initializeRest() {
    empManager = (EMPManager) objectsManager;
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) {
      if (empManager.isGolden) {
        GoldManager.gm.add(transform.position, cubesWhenDestroy(), false);
      }
    }
  }

  override protected void afterEncounter() {
    empManager.generateForceField();
  }

  override public string getManager() {
    return "EMPManager";
  }

  override public int cubesWhenDestroy() {
    return 50;
  }
}
