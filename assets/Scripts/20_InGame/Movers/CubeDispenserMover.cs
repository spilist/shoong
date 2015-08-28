using UnityEngine;
using System.Collections;

public class CubeDispenserMover : ObjectsMover {
  private CubeDispenserManager cdm;
  private ParticleSystem reaction;

  protected override void initializeRest() {
    canBeMagnetized = false;
    cdm = GameObject.Find("Field Objects").GetComponent<CubeDispenserManager>();
    reaction = transform.Find("Reaction").GetComponent<ParticleSystem>();
  }

  protected override float getSpeed() {
    return 0;
  }

  protected override float getTumble() {
    return 0.5f;
  }

  override protected void doSomethingSpecial(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      PlayerMover playerMover = player.GetComponent<PlayerMover>();
      if (playerMover.isUsingRainbow()) {
        playerMover.goodPartsEncounter(this, cdm.cubesPerContact * 4);
      } else {
        playerMover.contactCubeDispenser(transform, cdm.cubesPerContact, collision, cdm.reboundDuring);
        cdm.contact();
        reaction.Play();
      }
    }
  }

  override public void destroyObject() {
    cdm.destroyInstances();
  }

  override public string getManager() {
    return "CubeDispenserManager";
  }

  override public void encounterPlayer() {
    cdm.destroyInstances();
  }
}
