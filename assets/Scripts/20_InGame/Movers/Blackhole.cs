using UnityEngine;
using System.Collections;

public class Blackhole : MonoBehaviour {
  private BlackholeManager blm;
  private ScoreManager scoreManager;

  private PlayerMover player;

  private bool isQuitting = false;

  void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 5;

    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
	}

  void OnCollisionEnter(Collision collision) {
    GameObject other = collision.collider.gameObject;
    if (other.tag == "ContactCollider") {
      if (player.isUnstoppable()) {
        player.contactBlackholeWhileUnstoppable(collision);
        Destroy(gameObject);
      } else if (player.isUsingRainbow()) {
        player.contactBlackholeWhileRainbow(collision);
        Destroy(gameObject);
      } else {
        scoreManager.gameOver();
      }
    } else {
      ObjectsMover mover = other.GetComponent<ObjectsMover>();
      mover.destroyObject(false);
    }
  }

  void OnApplicationQuit() {
    isQuitting = true;
  }

  void OnDestroy() {
    if (isQuitting) return;
    if (scoreManager.isGameOver()) return;
    if (player.isUsingBlackhole()) return;

    blm.run();
  }
}
