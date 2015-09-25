using UnityEngine;
using System.Collections;

public class DoppleMover : ObjectsMover {
  DoppleManager dpm;

  override protected void initializeRest() {
    dpm = (DoppleManager) objectsManager;
    canBeMagnetized = false;
  }

  override public string getManager() {
    return "DoppleManager";
  }

  override protected void afterEncounter() {
    if (player.isOnPowerBoost()) return;

    Instantiate(dpm.forceFieldPrefab, transform.position, Quaternion.identity);
    Camera.main.GetComponent<CameraMover>().setSlowly(true);
  }
}
