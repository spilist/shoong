﻿using UnityEngine;
using System.Collections;

public class BlackholeMover : ObjectsMover {
  private ScoreManager scoreManager;

  override protected void initializeRest() {
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    canBeMagnetized = false;
	}

  void OnCollisionEnter(Collision collision) {
    GameObject other = collision.collider.gameObject;
    if (other.tag == "ContactCollider") {
      if (dangerous()) {
        scoreManager.gameOver("Blackhole");
      } else {
        player.contactBlackhole(collision);
        encounterPlayer();
      }
    } else {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      mover.destroyObject(false);
    }
  }

  override public bool dangerous() {
    if (player.isUsingDopple()) return true;
    else if (player.isAfterStrengthen() || player.isUnstoppable() || player.isUsingRainbow() || player.isRebounding()) return false;
    else return true;
  }

  override public string getManager() {
    return "BlackholeManager";
  }

  override public int cubesWhenDestroy() {
    return cubesWhenEncounter();
  }
}
