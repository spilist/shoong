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
        empManager.gcCount.add(cubesWhenDestroy(), false);
      } else if (empManager.isSuper) {
        SuperheatManager.sm.addGuageWithEffect(cubesWhenDestroy());
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
