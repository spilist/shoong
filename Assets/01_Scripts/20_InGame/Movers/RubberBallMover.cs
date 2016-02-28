using UnityEngine;
using System.Collections;

public class RubberBallMover : ObjectsMover {
  private RubberBallManager rbm;
  private GameObject reaction;
  private Animation beatAnimation;

  override public string getManager() {
    return "RubberBallManager";
  }

  protected override void initializeRest() {
    rbm = (RubberBallManager)objectsManager;
    reaction = transform.Find("Reaction").gameObject;
    canBeMagnetized = false;
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  protected override void afterEnable() {
    transform.localScale = rbm.objScale * Vector3.one;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterCollidePlayer() {
    reaction.SetActive(false);
    reaction.SetActive(true);
  }
}
