using UnityEngine;
using System.Collections;

public class SuperheatPartManager : ObjectsManager {
  public int guageForEncounter = 100;

  override public void initRest() {
    run();
  }

  public void add(bool withEffect = true) {
    if (player.isOnSuperheat()) return;
    SuperheatManager.sm.addGuageWithEffect(guageForEncounter);
    if (withEffect) {
      GameObject obj = getPooledObj(objEncounterEffectPool, objEncounterEffect, player.transform.position);
      obj.SetActive(true);
    }
  }
}
