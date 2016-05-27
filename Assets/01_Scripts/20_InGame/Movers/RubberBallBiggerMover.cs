using UnityEngine;
using System.Collections;

public class RubberBallBiggerMover : ObjectsMover {
  private RubberBallBiggerManager rbm;
  private AudioSource reaction;
  private Animation beatAnimation;

  override public string getManager() {
    return "RubberBallBiggerManager";
  }

  protected override void initializeRest() {
    rbm = (RubberBallBiggerManager)objectsManager;
    reaction = transform.Find("Reaction").GetComponent<AudioSource>();
    canBeMagnetized = false;
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterCollidePlayer(bool effect) {
    reaction.Play();
  }
}
