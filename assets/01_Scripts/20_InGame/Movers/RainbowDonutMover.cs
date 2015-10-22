using UnityEngine;
using System.Collections;

public class RainbowDonutMover : ObjectsMover {
  private RainbowDonutsManager rdm;
  private bool rotatingFast = false;

  override protected void initializeRest() {
    canBeMagnetized = false;
    rdm = (RainbowDonutsManager) objectsManager;
  }

  override protected void afterEnable() {
    rotatingFast = false;
    GetComponent<Collider>().enabled = true;
    GetComponent<Rigidbody>().isKinematic = false;
  }

  override public void encounterPlayer(bool destroy = true) {
    if (player.isOnSuperheat()) return;
    GetComponent<Collider>().enabled = false;
    rdm.startRidingRainbow();
    StartCoroutine("rideRainbow");
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) {
      if (rdm.isGolden) {
        rdm.gcCount.add(cubesWhenEncounter() * rdm.numRoadRides, false);
      } else if (rdm.isSuper) {
        rdm.superheat.addGuageWithEffect(rdm.guageAmountSuper * rdm.numRoadRides);
      }
    }
  }

  override public string getManager() {
    return "RainbowDonutsManager";
  }

  IEnumerator rideRainbow() {
    rotatingFast = true;
    GetComponent<Rigidbody>().isKinematic = true;
    yield return new WaitForSeconds(rdm.rotateDuring);
    gameObject.SetActive(false);
  }

  void Update() {
    if (rotatingFast) {
      transform.Rotate(-Vector3.forward * Time.deltaTime * rdm.rotateAngularSpeed, Space.World);
    }
  }

  override public int cubesWhenDestroy() {
    return cubesWhenEncounter() * rdm.numRoadRides;
  }
}
