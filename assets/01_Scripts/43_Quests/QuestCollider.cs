using UnityEngine;
using System.Collections;

public class QuestCollider : MonoBehaviour {
  public PlayerMover player;
  public float collideRadius = 1.5f;

  void OnEnable() {
    GetComponent<SphereCollider>().radius = collideRadius;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big") {
      other.GetComponent<AsteroidMover>().nearPlayer();
    } else if (other.tag == "Obstacle") {
      other.GetComponent<MeteroidMover>().nearPlayer();
    }
  }

  void OnTriggerExit(Collider other) {
   if (other.tag == "Obstacle_big") {
      other.GetComponent<AsteroidMover>().nearPlayer(false);
    } else if (other.tag == "Obstacle") {
      if (other.GetComponent<MeteroidMover>().isAlreadyChecked() || player.isRidingMonster()) return;

      other.GetComponent<MeteroidMover>().nearPlayer(false);
      QuestManager.qm.addCountToQuest("AvoidFallingStar");
      player.showEffect("Whew");
    }
  }
}
