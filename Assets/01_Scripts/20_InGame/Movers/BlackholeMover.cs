using UnityEngine;
using System.Collections;

public class BlackholeMover : ObjectsMover {
  override protected void initializeRest() {
    canBeMagnetized = false;
	}

  void OnCollisionEnter(Collision collision) {
    GameObject other = collision.collider.gameObject;
    if (other.tag == "ContactCollider") {
      if (dangerous()) {
        ScoreManager.sm.gameOver("Blackhole");
      } else {
        if (!player.isUsingSolar()) player.contactBlackhole(collision);
        encounterPlayer();
      }
    } else {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
    }
  }

  override public bool dangerous() {
    if (player.isAfterStrengthen() || player.isUnstoppable() || player.isUsingRainbow() || player.isRebounding() || player.isUsingSolar() || player.isDashing()) return false;
    else return true;
  }

  override public string getManager() {
    return "BlackholeManager";
  }
}
