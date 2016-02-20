using UnityEngine;
using System.Collections;

public class RubberBallMover : ObjectsMover {
  private RubberBallManager rbm;
  private ParticleSystem reaction;

  override public string getManager() {
    return "RubberBallManager";
  }

  protected override void initializeRest() {
    rbm = (RubberBallManager)objectsManager;
    reaction = transform.Find("Reaction").GetComponent<ParticleSystem>();
    canBeMagnetized = false;
  }

  protected override void afterEnable() {
    transform.localScale = rbm.objScale * Vector3.one;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterCollidePlayer() {
    reaction.Play();
    reaction.GetComponent<AudioSource>().Play();
  }
}
