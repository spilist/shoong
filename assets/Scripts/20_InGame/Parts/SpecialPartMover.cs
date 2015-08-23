using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
  }

  override public void destroyObject() {
    Destroy(gameObject);
    objectsManager.run();
  }

  override public void encounterPlayer() {
    Destroy(gameObject);
  }
}
