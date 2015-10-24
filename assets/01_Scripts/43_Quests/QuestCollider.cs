using UnityEngine;
using System.Collections;

public class QuestCollider : MonoBehaviour {
  public float collideRadius = 1.5f;

  void OnEnable() {
    GetComponent<SphereCollider>().radius = collideRadius;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Obstacle_big") {
      other.GetComponent<AsteroidMover>().nearPlayer();
    } else if (other.tag == "Obstacle_small") {
      other.GetComponent<SmallAsteroidMover>().nearPlayer();
    } else if (other.tag == "Obstacle") {
      other.GetComponent<MeteroidMover>().nearPlayer();
    }
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Obstacle_big") {
      other.GetComponent<AsteroidMover>().nearPlayer(false);
    } else if (other.tag == "Obstacle_small") {
      other.GetComponent<SmallAsteroidMover>().nearPlayer(false);
    } else if (other.tag == "Obstacle") {
      if (other.GetComponent<MeteroidMover>().isAlreadyChecked() || Player.pl.isRidingMonster()) return;

      other.GetComponent<MeteroidMover>().nearPlayer(false);
      Player.pl.showEffect("Whew");
    }
  }
}
