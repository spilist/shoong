using UnityEngine;
using System.Collections;

public class TimeMonsterMover : ObjectsMover {

	override public string getManager() {
    return "TimeMonsterManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  protected override void normalMovement() {
    rb.velocity = getDirection() * getSpeed();
  }

  override public bool dangerous() {
    return true;
  }

  override public void destroyObject(bool destroyEffect = true, bool byPlayer = false) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    if (destroyEffect && objectsManager.objDestroyEffect != null) {
      Instantiate(objectsManager.objDestroyEffect, transform.position, transform.rotation);
    }

    player.destroyObject(tag);
  }
}
