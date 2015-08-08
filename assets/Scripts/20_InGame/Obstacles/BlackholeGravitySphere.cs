using UnityEngine;
using System.Collections;

public class BlackholeGravitySphere : MonoBehaviour {
  private PlayerMover player;
  private GameObject blackhole;
  private Transform targetTransform;
  private bool startFollow = false;

	void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();
	}

  public void setBlackhole(GameObject obj) {
    blackhole = obj;
    targetTransform = blackhole.GetComponent<Transform>();
    startFollow = true;
  }

  void LateUpdate() {
    if (startFollow) {
      if (blackhole == null) {
        Destroy(gameObject);
      } else {
        transform.position = new Vector3 (targetTransform.position.x, transform.position.y, targetTransform.position.z);
      }
    }
  }

  void OnTriggerEnter(Collider other) {
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
      player.outsideBlackhole();
    }
  }
}
