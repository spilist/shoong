﻿using UnityEngine;
using System.Collections;

public class RainbowDonutMover : ObjectsMover {
  private RainbowDonutsManager rdm;
  private bool rotatingFast = false;

  override protected void initializeRest() {
    canBeMagnetized = false;
    rdm = (RainbowDonutsManager) objectsManager;
  }

	override public void destroyObject(bool destroyEffect = true) {
    Destroy(gameObject);
    objectsManager.run();
  }

  override public void encounterPlayer() {
    rdm.startRidingRainbow();
    GetComponent<Collider>().enabled = false;
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

  override public int cubesWhenEncounter() {
    return rdm.cubesPerRide;
  }
}
