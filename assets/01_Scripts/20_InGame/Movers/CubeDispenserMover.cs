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

        reaction.Play();
        AudioSource sound = reaction.GetComponent<AudioSource>();
        sound.pitch = cdm.pitchStart + cdm.getComboCount() * cdm.pitchIncrease;
        sound.Play ();

        cdm.contact();
      }
    }
  }

  override public void destroyObject(bool destroyEffect = true) {
    cdm.destroyInstances();
  }

  override public string getManager() {
    return "CubeDispenserManager";
  }

  override public void encounterPlayer() {
    cdm.destroyInstances();
  }
}
