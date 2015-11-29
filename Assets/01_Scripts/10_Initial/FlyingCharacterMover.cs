using UnityEngine;
using System.Collections;

public class FlyingCharacterMover : MonoBehaviour {
	private Rigidbody rb;
  private bool randomMoving = false;

  FlyingCharacters flc;

  float boosterSpeed = 0;
  int minBoosterSpeed;
  int maxBoosterSpeed;
  int decreaseBase;
  int decreasePerTime;
  float delay;
  float delayCount = 0;
  bool isWaiting = false;
  ParticleSystem booster;
  AudioSource boosterSound;

  Vector3 direction;
  float baseSpeed;
  float tumble;
  float speed;

  void OnEnable() {
    randomMoving = false;
    isWaiting = false;
    delayCount = 0;
    boosterSpeed = 0;
  }

  public void run(FlyingCharacters flc, Vector3 dir) {
    this.flc = flc;
    rb = GetComponent<Rigidbody>();
    tumble = flc.tumble;
    baseSpeed = flc.baseSpeed;

    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = dir * flc.speed;

    booster = transform.Find("Booster").GetComponent<ParticleSystem>();
    boosterSound = booster.GetComponent<AudioSource>();

    minBoosterSpeed = flc.minBoosterAmount;
    maxBoosterSpeed = flc.maxBoosterAmonut;
    decreaseBase = flc.boosterSpeedDecreaseBase;
    decreasePerTime = flc.boosterSpeedDecreasePerTime;
    delay = flc.delayAfterMove;
  }

  void OnTriggerEnter(Collider other) {
    string tag = other.tag;

    if (tag == "FlyingBoundary") {
      if (randomMoving) {
        gameObject.SetActive(false);
        flc.activeCount--;
      }
    } else if (tag == "ChangeBehavior") {
      randomMoving = true;
    }
  }

  void FixedUpdate() {
    if (randomMoving) {
      speed = baseSpeed + boosterSpeed;

      if (boosterSpeed > 0) {
        boosterSpeed -= speed / decreaseBase + decreasePerTime * Time.deltaTime;
      }

      if (boosterSpeed <= 0 && !isWaiting){
        boosterSpeed = 0;
        isWaiting = true;
      }

      if (isWaiting) {
        if (delayCount < delay) delayCount += Time.deltaTime;
        else {
          isWaiting = false;
          delayCount = 0;
          shootBooster();
        }
      }

      rb.velocity = direction * speed;
    }
  }

  void shootBooster() {
    rb.angularVelocity = Random.onUnitSphere * tumble;
    booster.Play();
    boosterSound.Play();

    Vector2 randomV = Random.insideUnitCircle;
    direction = new Vector3(randomV.x, 0, randomV.y).normalized;

    boosterSpeed += Random.Range(minBoosterSpeed, maxBoosterSpeed);
  }
}
