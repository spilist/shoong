using UnityEngine;
using System.Collections;

public class SuperheatPartMover : ObjectsMover {
  private SuperheatPartManager spm;

  protected override void initializeRest() {
    spm = (SuperheatPartManager)objectsManager;
  }

  override protected void afterEncounter() {
    spm.add(false);
    objectsManager.run();
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) {
      spm.add();
    }
  }

  override public string getManager() {
    return "SuperheatPartManager";
  }
}
