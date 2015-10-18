using UnityEngine;
using System.Collections;

public class EMPMover : ObjectsMover {
  EMPManager empManager;

	override protected void initializeRest() {
    empManager = (EMPManager) objectsManager;
  }

  override protected void afterEncounter() {
    empManager.generateForceField();
  }

  override public string getManager() {
    return "EMPManager";
  }
}
