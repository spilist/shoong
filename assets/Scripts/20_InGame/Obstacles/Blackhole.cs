using UnityEngine;
using System.Collections;

public class Blackhole : MonoBehaviour {
  public ParticleSystem obstacleDestroy;

  private FieldObjectsManager fom;
  private ComboPartsManager cpm;
  private BlackholeManager blm;
  private CubeDispenserManager cdm;
  private GameOver gameOver;

  private PlayerMover player;

  private bool isQuitting = false;

  void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 5;

    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    cdm = GameObject.Find("Field Objects").GetComponent<CubeDispenserManager>();
    gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
	}

  void OnCollisionEnter(Collision collision) {
    GameObject other = collision.collider.gameObject;
    if (other.tag == "SpecialPart") {
      fom.spawnSpecial();
    } else if (other.tag == "ComboPart") {
      cpm.destroyInstances();
    } else if (other.tag == "Obstacle" || other.tag == "Obstacle_big" || other.tag == "Monster") {
      Destroy(other.gameObject);
    } else if (other.tag == "ContactCollider") {
      if (player.isUnstoppable()) {
        player.contactBlackholeWhileUnstoppable(collision);
        Destroy(gameObject);
      } else {
        gameOver.run();
      }
    } else if (other.tag == "CubeDispenser") {
      cdm.startRespawn();
    } else {
      Destroy(other.gameObject);
    }
  }

  void OnApplicationQuit() {
    isQuitting = true;
  }

  void OnDestroy() {
    if (isQuitting) return;
    if (gameOver.isOver()) return;
    if (player.isExitedBlackhole()) return;

    blm.run();
  }
}
