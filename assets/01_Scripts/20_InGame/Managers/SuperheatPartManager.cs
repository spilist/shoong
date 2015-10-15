using UnityEngine;
using System.Collections;

public class SuperheatPartManager : ObjectsManager {
  public Superheat superheat;
  public int guageForEncounter = 100;

  override public void initRest() {
    run();
  }

  public void add(bool withEffect = true) {
    superheat.addGuageWithEffect(guageForEncounter);
    if (withEffect) {
      Instantiate(objEncounterEffect, player.transform.position, Quaternion.identity);
    }
  }
}
