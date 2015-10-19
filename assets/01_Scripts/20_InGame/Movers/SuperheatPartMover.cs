using UnityEngine;
using System.Collections;

public class SuperheatPartMover : ObjectsMover {
  private SuperheatPartManager superm;

  protected override void initializeRest() {
    superm = (SuperheatPartManager)objectsManager;
  }

  override protected void afterEncounter() {
    superm.add(false);
    objectsManager.run();
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) {
      superm.add();
    }
  }

  override public string getManager() {
    return "SuperheatPartManager";
  }
}
