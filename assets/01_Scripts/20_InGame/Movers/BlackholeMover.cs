using UnityEngine;
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

  override protected void afterDestroy(bool byPlayer) {
    Destroy(blm.blackholeGravity);
  }

  override protected void afterEncounter() {
    QuestManager.qm.addCountToQuest("ExitBlackhole");

    Destroy(blm.blackholeGravity);
  }

  override public bool dangerous() {
    if (player.isUsingRainbow()) return false;
    return true;
  }

  override public string getManager() {
    return "BlackholeManager";
  }
}
