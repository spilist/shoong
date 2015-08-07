using UnityEngine;
using System.Collections;

public class BlackholeGravitySphere : MonoBehaviour {
  private PlayerMover player;

	void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
	}

  void OnTriggerEnter(Collider other) {
    // Debug.Log(other.tag + "is in blackhole gravity");
    if (other.tag == "Player") {
      player.insideBlackhole();
    } else if (other.tag == "Obstacle_big") {
      other.gameObject.GetComponent<BigObstaclesMover>().insideBlackhole();
    } else if (other.tag == "Obstacle") {
      other.gameObject.GetComponent<ObstaclesMover>().insideBlackhole();
    } else if (other.tag == "Monster") {
      other.gameObject.GetComponent<MonsterMover>().insideBlackhole();
    } else {
      other.gameObject.GetComponent<FieldObjectsMover>().insideBlackhole();
    }
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player" && !player.isUnstoppable()) {
      // Debug.Log(other.tag + "is outside blackhole gravity");
      player.outsideBlackhole();
    }
  }
}
