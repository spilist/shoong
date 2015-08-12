﻿using UnityEngine;
using System.Collections;

public class CubeDispenserMover : MonoBehaviour {
  private CubeDispenserManager cdm;
  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;

  private PlayerMover player;
  private Vector3 direction;


	void Start () {
		GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 0.5f;
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    cdm = GameObject.Find("Field Objects").GetComponent<CubeDispenserManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();

	}

  void FixedUpdate () {

    if (isInsideBlackhole) {
      if (blackhole == null) {
        Destroy(gameObject);
        return;
      }
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    }
  }

	void OnCollisionEnter(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      player.contactCubeDispenser(transform, cdm.cubesPerContact, collision, cdm.reboundDuring);
      cdm.contact();
    }
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }
}
