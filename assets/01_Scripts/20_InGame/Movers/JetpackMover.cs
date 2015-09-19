using UnityEngine;
using System.Collections;

public class JetpackMover : ObjectsMover {
  JetpackManager jpm;

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

  override protected void initializeRest() {
    jpm = (JetpackManager) objectsManager;
    canBeMagnetized = false;

    booster = transform.Find("Booster").GetComponent<ParticleSystem>();
    boosterSound = booster.GetComponent<AudioSource>();

    minBoosterSpeed = jpm.minBoosterAmount;
    maxBoosterSpeed = jpm.maxBoosterAmonut;
    decreaseBase = jpm.boosterSpeedDecreaseBase;
    decreasePerTime = jpm.boosterSpeedDecreasePerTime;
    delay = jpm.delayAfterMove;
  }

  override public string getManager() {
    return "JetpackManager";
  }

  override protected void normalMovement() {
    speed = getSpeed() + boosterSpeed;

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

  public void shootBooster() {
    rb.angularVelocity = Random.onUnitSphere * tumble;
    booster.Play();
    boosterSound.Play();

    direction = getDirection();

    boosterSpeed += Random.Range(minBoosterSpeed, maxBoosterSpeed);
  }

}
