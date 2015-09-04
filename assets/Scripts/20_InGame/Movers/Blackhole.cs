using UnityEngine;
using System.Collections;

public class Blackhole : MonoBehaviour {
  public ParticleSystem obstacleDestroy;

  private BlackholeManager blm;
  private GameOver gameOver;

  private PlayerMover player;

  private bool isQuitting = false;

  void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 5;

    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
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
        gameOver.run();
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
    if (gameOver.isOver()) return;
    if (player.isUsingBlackhole()) return;

    blm.run();
  }
}
