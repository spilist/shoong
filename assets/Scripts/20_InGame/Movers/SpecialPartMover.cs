using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
  }

  override public void destroyObject(bool destroyEffect = true) {
    Destroy(gameObject);
    objectsManager.run();
  }

  override public void encounterPlayer() {
    Destroy(gameObject);
  }

  override public string getManager() {
    return "SpecialPartsManager";
  }
}
