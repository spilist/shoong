using UnityEngine;
using System.Collections;

public class MonsterMover : MonoBehaviour {
  private float speed_chase;
  private float speed_runaway;
  private float tumble;
  private Vector3 direction;

  private FieldObjectsManager fom;
  private MonsterManager monm;
  private ComboPartsManager cpm;

  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;

  private PlayerMover player;
  private GameOver gameOver;
  private bool isQuitting = false;
	private float distance;

	void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    monm = GameObject.Find("Field Objects").GetComponent<MonsterManager>();
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    shrinkedScale = transform.localScale.x;

    speed_chase = monm.speed_chase;
    speed_runaway = monm.speed_runaway;
    tumble = monm.tumble;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;
    GetComponent<Rigidbody>().velocity = direction * speed_chase;

    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();

    Destroy(gameObject, Random.Range(monm.minLifeTime, monm.maxLifeTime));
	}

	void FixedUpdate () {
    if (isInsideBlackhole) {
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else {
      direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
			distance = direction.magnitude;
      direction /= direction.magnitude;

			if(distance>140){
				GetComponent<Rigidbody>().velocity = direction * speed_chase*2;

			}else if(distance<140 & distance>80){
				GetComponent<Rigidbody>().velocity = direction * speed_runaway;

			}else{
				if (player.isUnstoppable()) {
					GetComponent<Rigidbody>().velocity = -direction * speed_runaway;
				} else {
					GetComponent<Rigidbody>().velocity = direction * speed_chase;
				}
			}



      
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
      fom.spawnSpecial();
    } else if (colliderTag == "ComboPart") {
      cpm.destroyInstances();
    }
  }

  void OnApplicationQuit() {
    isQuitting = true;
  }

  void OnDestroy() {
    if (isQuitting) return;
    if (gameOver.isOver()) return;

    monm.stopWarning();
    monm.run();
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = GameObject.Find("Blackhole");
  }
}
