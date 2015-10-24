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
}
