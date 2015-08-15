using UnityEngine;
using System.Collections;

public class CubeDispenserMover : ObjectsMover {
  private CubeDispenserManager cdm;

  protected override void initializeRest() {
    canBeMagnetized = false;
    cdm = GameObject.Find("Field Objects").GetComponent<CubeDispenserManager>();
  }

  protected override float getSpeed() {
    return 0;
  }

  protected override float getTumble() {
    return 0.5f;
  }

  override protected void doSomethingSpecial(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      player.GetComponent<PlayerMover>().contactCubeDispenser(transform, cdm.cubesPerContact, collision, cdm.reboundDuring);
      cdm.contact();
    }
  }

  override public void destroyObject() {
    Destroy(gameObject);
    cdm.startRespawn();
  }
}
