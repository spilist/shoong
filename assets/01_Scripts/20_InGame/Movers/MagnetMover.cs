using UnityEngine;
using System.Collections;

public class MagnetMover : ObjectsMover {
  private MagnetManager magnetManager;
  bool pulling = true;
  float pullPushDuration;
  float pauseDuration;
  public int powerToParts;
  public int powerToPlayer_pull;
  public int powerToPlayer_push;

  GameObject pull;
  GameObject pullBody;
  GameObject push;
  GameObject pushBody;

  override protected void initializeRest() {
    magnetManager = (MagnetManager)objectsManager;
    canBeMagnetized = false;

    pullPushDuration = magnetManager.pullPushDuration;
    pauseDuration = magnetManager.pauseDuration;
    powerToParts = magnetManager.powerToParts;
    powerToPlayer_push = magnetManager.powerToPlayer_push;
    powerToPlayer_pull = magnetManager.powerToPlayer_pull;

    pull = transform.Find("Pull").gameObject;
    push = transform.Find("Push").gameObject;
    pullBody = transform.Find("BodyParticles/PullBody").gameObject;
    pushBody = transform.Find("BodyParticles/PushBody").gameObject;
    StartCoroutine("pullPush");
  }

  IEnumerator pullPush() {
    while (true) {
      pulling = true;
      pull.SetActive(true);
      pullBody.SetActive(true);
      pushBody.SetActive(false);
      yield return new WaitForSeconds(pullPushDuration);

      pulling = false;
      pull.SetActive(false);
      pushBody.SetActive(true);
      yield return new WaitForSeconds(pauseDuration);

      push.SetActive(true);
      pullBody.SetActive(false);
      pushBody.SetActive(true);
      yield return new WaitForSeconds(pullPushDuration);

      push.SetActive(false);
      pullBody.SetActive(true);
      yield return new WaitForSeconds(pauseDuration);
    }
  }

  override public string getManager() {
    return "MagnetManager";
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return pulling;
  }

  // override protected void afterDestroy(bool byPlayer) {
  //   player.magnetizeEnd();
  // }

  // override protected void afterEncounter() {
  //   player.magnetizeEnd();
  // }
}
