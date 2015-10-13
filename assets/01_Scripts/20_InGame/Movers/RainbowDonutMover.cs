using UnityEngine;
using System.Collections;

public class RainbowDonutMover : ObjectsMover {
  private RainbowDonutsManager rdm;
  private bool rotatingFast = false;

  override protected void initializeRest() {
    canBeMagnetized = false;
    rdm = (RainbowDonutsManager) objectsManager;
  }

  override public void encounterPlayer(bool destroy = true) {
    if (player.isOnSuperheat()) return;
    GetComponent<Collider>().enabled = false;
    rdm.startRidingRainbow();
    StartCoroutine("rideRainbow");
  }

  override public string getManager() {
    return "RainbowDonutsManager";
  }

  IEnumerator rideRainbow() {
    rotatingFast = true;
    GetComponent<Rigidbody>().isKinematic = true;
    yield return new WaitForSeconds(rdm.rotateDuring);
    Destroy(gameObject);
  }

  void Update() {
    if (rotatingFast) {
      transform.Rotate(-Vector3.forward * Time.deltaTime * rdm.rotateAngularSpeed, Space.World);
    }
  }
}
