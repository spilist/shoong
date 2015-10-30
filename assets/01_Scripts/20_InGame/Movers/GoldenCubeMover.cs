using UnityEngine;
using System.Collections;

public class GoldenCubeMover : ObjectsMover {
  private GoldenCubeManager gcm;
  private bool detected = false;
  private bool noRespawn = false;

  protected override void initializeRest() {
    gcm = (GoldenCubeManager)objectsManager;
  }

  override protected void afterEnable() {
    detected = false;
    noRespawn = false;
  }

  public void setNoRespawn() {
    noRespawn = true;
  }

  protected override bool beforeCollide(Collision collision) {
    if (collision.collider.tag == "CubeDispenser") {
      processCollision(collision);
      return false;
    } else {
      return true;
    }
  }

  protected override void normalMovement() {
    direction = player.transform.position - transform.position;
    float distance = direction.magnitude;
    direction /= distance;
    if (distance < gcm.detectDistance) {
      rb.velocity = -direction * gcm.speed;
      if (!detected) {
        detected = true;
      }
    }
  }

  override public void destroyObject(bool destroyEffect = true, bool byPlayer = false) {

    gameObject.SetActive(false);

    if (destroyEffect) {
      showDestroyEffect(byPlayer);
    }

    if (noRespawn) return;

    if (byPlayer) {
      objectsManager.run();
    } else {
      objectsManager.runImmediately();
    }
  }

  override protected void afterEncounter() {
    GoldManager.gm.add(transform.position, gcm.cubesWhenEncounter());

    if (noRespawn) return;

    objectsManager.run();
  }

  override public string getManager() {
    return "GoldenCubeManager";
  }

  override public bool noCubesByDestroy() {
    return true;
  }

  override protected float getSpeed() {
    return 0;
  }
}
