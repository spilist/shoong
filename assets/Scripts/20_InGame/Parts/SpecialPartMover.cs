using UnityEngine;
using System.Collections;

public class SpecialPartMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
  }

  override public void destroyObject() {
    fom.spawnSpecial();
  }

  override public void encounterPlayer() {
    Destroy(gameObject);
  }
}
