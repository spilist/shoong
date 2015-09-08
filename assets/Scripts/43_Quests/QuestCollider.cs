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
      player.nearAsteroid();
      other.GetComponent<BigObstaclesMover>().nearPlayer();
    } else if (other.tag == "Obstacle") {
      other.GetComponent<ObstaclesMover>().nearPlayer();
    }
  }

  void OnTriggerExit(Collider other) {
   if (other.tag == "Obstacle_big") {
      player.nearAsteroid(false);
      other.GetComponent<BigObstaclesMover>().nearPlayer(false);
    } else if (other.tag == "Obstacle") {
      other.GetComponent<ObstaclesMover>().nearPlayer(false);
      QuestManager.qm.addCountToQuest("AvoidFallingStar");
    }
  }
}
