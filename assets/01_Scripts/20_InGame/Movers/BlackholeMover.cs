using UnityEngine;
using System.Collections;

public class BlackholeMover : ObjectsMover {
  private ScoreManager scoreManager;
  private bool isContact = false;

  override protected void initializeRest() {
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    canBeMagnetized = false;
	}

  void OnCollisionEnter(Collision collision) {
    GameObject other = collision.collider.gameObject;
    if (other.tag == "ContactCollider") {
      if (dangerous()) {
        scoreManager.gameOver();
      } else {
        player.contactBlackhole(collision);
        isContact = true;
        encounterPlayer();
      }
    } else {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      mover.destroyObject(false);
    }
  }

  override protected void afterDestroy() {
    Destroy(blm.blackholeGravity);
  }

  override protected void afterEncounter() {
    if (!isContact) QuestManager.qm.addCountToQuest("ExitBlackhole");

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
