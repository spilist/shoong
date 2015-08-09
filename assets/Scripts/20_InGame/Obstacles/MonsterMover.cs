using UnityEngine;
using System.Collections;

public class MonsterMover : MonoBehaviour {
  private float speed_chase;
  private float speed_runaway;
  private float speed_weaken;
  private float tumble;
  private Vector3 direction;

  private FieldObjectsManager fom;
  private MonsterManager monm;
  private ComboPartsManager cpm;
  private CubeDispenserManager cdm;

  private BlackholeManager blm;
  private GameObject blackhole;
  private bool isInsideBlackhole = false;
  private float shrinkedScale;
  private bool isMagnetized = false;

  private PlayerMover player;
  private GameOver gameOver;
  private bool isQuitting = false;
  private float distance;

  private float originalScale;
  private bool weak = false;
  private bool shrinking = true;

  public ParticleSystem aura;
  public ParticleSystem weakenAura;

	void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    monm = GameObject.Find("Field Objects").GetComponent<MonsterManager>();
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
    cdm = GameObject.Find("Field Objects").GetComponent<CubeDispenserManager>();

    shrinkedScale = transform.localScale.x;
    originalScale = shrinkedScale;

    speed_chase = monm.speed_chase;
    speed_runaway = monm.speed_runaway;
    speed_weaken = monm.speed_weaken;
    tumble = monm.tumble;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
    direction /= direction.magnitude;
    GetComponent<Rigidbody>().velocity = direction * speed_chase;

    player = GameObject.Find("Player").GetComponent<PlayerMover>();
    gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();

    StartCoroutine("weakened");
	}

  IEnumerator weakened() {
    yield return new WaitForSeconds(Random.Range(monm.minLifeTime, monm.maxLifeTime));
    weak = true;
    monm.stopWarning();
    weakenAura.Play();
    aura.Stop();
    GetComponent<Renderer>().material.SetColor("_OutlineColor", monm.weakenedOutlineColor);

    yield return new WaitForSeconds(monm.weakenDuration);
    Instantiate(monm.destroyEffect, transform.position, transform.rotation);
    Destroy(gameObject);
  }

  public bool isWeak() {
    return weak;
  }

	void FixedUpdate () {
    if (isInsideBlackhole) {
      if (blackhole == null) {
        Destroy(gameObject);
        return;
      }
      Vector3 heading = blackhole.transform.position - transform.position;
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * blm.gravity;

      shrinkedScale = Mathf.MoveTowards(shrinkedScale, 0f, Time.deltaTime);
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    } else {
      direction = GameObject.Find("Player").GetComponent<PlayerMover>().transform.position - transform.position;
			distance = direction.magnitude;
      direction /= direction.magnitude;

			if (distance > monm.detectDistance){
				GetComponent<Rigidbody>().velocity = direction * speed_chase * 2;

			} else if (isMagnetized) {
        GetComponent<Rigidbody>().velocity = direction * player.GetComponent<Rigidbody>().velocity.magnitude * fom.unstoppableFollowSpeed;
      } else if (weak) {
        GetComponent<Rigidbody>().velocity = -direction * speed_weaken;
      } else if (player.isUnstoppable()) {
        GetComponent<Rigidbody>().velocity = -direction * speed_runaway;
			} else {
        GetComponent<Rigidbody>().velocity = direction * speed_chase;
			}

      if (weak) {
        if (shrinking) {
          shrinkedScale = Mathf.MoveTowards(shrinkedScale, monm.shrinkUntil, Time.deltaTime * monm.shrinkSpeed);
          if (shrinkedScale == monm.shrinkUntil) shrinking = false;
        } else {
          shrinkedScale = Mathf.MoveTowards(shrinkedScale, originalScale, Time.deltaTime * monm.restoreSpeed);
          if (shrinkedScale == originalScale) shrinking = true;
        }
        transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
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
    } else if (colliderTag == "CubeDispenser") {
      cdm.startRespawn();
    }
  }

  void OnApplicationQuit() {
    isQuitting = true;
  }

  void OnDestroy() {
    if (isQuitting) return;
    if (gameOver.isOver()) return;
    if (player.isRiding()) return;

    monm.run();
  }

  public void insideBlackhole() {
    isInsideBlackhole = true;
    blackhole = blm.getBlackhole();
  }

  public void setMagnetized() {
    isMagnetized = true;
  }
}
