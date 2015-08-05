using UnityEngine;
using System.Collections;

public class MonsterMover : MonoBehaviour {
  private float speed_chase;
  private float speed_runaway;
  private float tumble;
  private Vector3 direction;

  private MonsterManager monm;
  private PlayerMover player;
  private GameOver gameOver;
  private bool isQuitting = false;

	void Start () {
    monm = GameObject.Find("Field Objects").GetComponent<MonsterManager>();
    speed_chase = monm.speed_chase;
    speed_runaway = monm.speed_runaway;
    tumble = monm.tumble;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;
    GetComponent<Rigidbody>().velocity = direction * speed_chase;

    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
	}

	void FixedUpdate () {
    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;

    if (player.isUnstoppable()) {
      GetComponent<Rigidbody>().velocity = -direction * speed_runaway;
    } else {
      GetComponent<Rigidbody>().velocity = direction * speed_chase;
    }
	}

  void OnCollisionEnter(Collision collision) {
    string colliderTag = collision.collider.tag;

    if (colliderTag == "Obstacle" || colliderTag == "Obstacle_big") {
      Instantiate(player.obstacleDestroy, collision.collider.transform.position, collision.collider.transform.rotation);
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "Part") {
      Destroy(collision.collider.gameObject);
    } else if (colliderTag == "SpecialPart") {
      GenerateNextSpecial gns = collision.collider.gameObject.GetComponent<GenerateNextSpecial>();
      if (gns.getComboCount() > 0) {
        // Player was trying to get it
        gns.destroySelf(true, true, false);
      } else {
        // Destroyed somewhere
        gns.destroySelf(false, false, true);
      }
    }
  }

  void OnApplicationQuit() {
    isQuitting = true;
  }

  void OnDestroy() {
    if (isQuitting || gameOver.isOver()) return;
    monm.run();
  }
}
