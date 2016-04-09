using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienshipMover : ObjectsMover {
  private AlienshipManager asm;
  private int shootLaserPer;
  private float chargeTime;
  private int headFollowingSpeed;
  private GameObject laserCanon;
  private float offScreenSpeedScale;
  private int detectDistance;

  private GameObject laserWarning;

  Quaternion rotation;
  float angleY;

  int shootingStatus = 0;
  float stayCount = 0;

  protected override void initializeRest() {
    asm = (AlienshipManager)objectsManager;
    canBeMagnetized = false;

    shootLaserPer = asm.shootLaserPer;
    chargeTime = asm.chargeTime;
    headFollowingSpeed = asm.headFollowingSpeed;
    offScreenSpeedScale = asm.offScreenSpeedScale;
    speed = asm.speed;
    detectDistance = asm.detectDistance;

    laserCanon = transform.Find("LaserCanon").gameObject;
  }

  protected override void afterEnable() {
    angleY = transform.eulerAngles.y;
    shootingStatus = 1;
    stayCount = 0;
    speed = asm.speed;
    freezePosition(false);
    laserCanon.SetActive(false);
  }

  protected override void normalMovement() {
    rb.velocity = direction * getSpeed();
    rb.angularVelocity = Vector3.zero;
  }

  override public string getManager() {
    return "AlienshipManager";
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected float getTumble() {
    return 0;
  }

  override protected float getSpeed() {
    float distance = Vector3.Distance(player.transform.position, transform.position);
    if (distance > detectDistance) {
      return speed + player.getSpeed() * offScreenSpeedScale;
    } else {
      return speed;
    }
  }

  override protected Vector3 getDirection() {
    Vector3 dir = player.transform.position - transform.position;
    return dir / dir.magnitude;
  }

  void Update() {
    if (shootingStatus == 1) {
      direction = getDirection();
      rotation = Quaternion.LookRotation(direction);
      float targetAngle = rotation.eulerAngles.y;
      if (Mathf.Abs(targetAngle - angleY) > 180) targetAngle -= 360;
      angleY = Mathf.MoveTowards(angleY, targetAngle, Time.deltaTime * headFollowingSpeed);
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, transform.eulerAngles.z);

      transform.Rotate(0, 0, Time.deltaTime * asm.tumble);

      if (stayCount < shootLaserPer) {
        stayCount += Time.deltaTime;
      } else {
        stayCount = 0;
        shootingStatus++;
        freezePosition(true);
        laserCanon.SetActive(true);
        laserWarning = asm.getLaserWarning(laserCanon.transform.position);
        laserWarning.SetActive(true);
        laserWarning.transform.eulerAngles = new Vector3(0, angleY - 90, 0);
      }
    } else if (shootingStatus == 2) {
      if (ScoreManager.sm.isGameOver()) {
        shootingStatus = 0;
        return;
      }

      if (stayCount < chargeTime) {
        stayCount += Time.deltaTime;
      } else {
        stayCount = 0;
        GameObject laser = asm.getLaser(laserWarning.transform.position);
        laser.SetActive(true);
        laser.GetComponent<AlienshipLaserMover>().set(laserWarning.transform.eulerAngles.y, asm, this);
        laserWarning.SetActive(false);
        shootingStatus = 1;
      }
    }
  }

  public void laserEnd() {
    if (ScoreManager.sm.isGameOver()) return;

    shootingStatus = 1;
    if (rb != null) freezePosition(false);
    laserCanon.SetActive(false);
  }

  override protected void afterEncounter() {
    base.afterEncounter();
    asm.run();
  }

  void OnDisable() {
    if (laserWarning != null) laserWarning.SetActive(false);
  }

  void freezePosition(bool freeze) {
    if (freeze) {
      rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
    } else {
      rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
  }
}
