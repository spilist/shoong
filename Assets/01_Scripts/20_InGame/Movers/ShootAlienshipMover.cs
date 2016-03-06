using UnityEngine;
using System.Collections;

public class ShootAlienshipMover : ObjectsMover {
  ShootAlienshipManager sam;
  GameObject gunHead;

  int numBullets;
  float shootInterval;
  float nonShootDuration;

  int headFollowingSpeed;
  float offScreenSpeedScale;
  int detectDistance;

  Quaternion rotation;
  float angleY;

  float stayCount = 0;

  protected override void initializeRest() {
    sam = (ShootAlienshipManager)objectsManager;
    canBeMagnetized = false;

    numBullets = sam.numBullets;
    shootInterval = sam.shootInterval;
    nonShootDuration = sam.nonShootDuration;

    headFollowingSpeed = sam.headFollowingSpeed;
    offScreenSpeedScale = sam.offScreenSpeedScale;
    speed = sam.speed;
    detectDistance = sam.detectDistance;

    gunHead = transform.Find("GunHead").gameObject;
  }

  protected override void afterEnable() {
    angleY = transform.eulerAngles.y;
    stayCount = 0;
    speed = sam.speed;
    StopCoroutine("shootBullets");
    StartCoroutine("shootBullets");
  }

  protected override void normalMovement() {
    rb.velocity = direction * getSpeed();
    rb.angularVelocity = Vector3.zero;
  }

  override public string getManager() {
    return "ShootAlienshipManager";
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
    direction = getDirection();
    rotation = Quaternion.LookRotation(direction);
    float targetAngle = rotation.eulerAngles.y;
    if (Mathf.Abs(targetAngle - angleY) > 180) targetAngle -= 360;
    angleY = Mathf.MoveTowards(angleY, targetAngle, Time.deltaTime * headFollowingSpeed);
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, transform.eulerAngles.z);

    transform.Rotate(0, 0, Time.deltaTime * sam.tumble);
  }

  override protected void afterEncounter() {
    base.afterEncounter();
    sam.run();
  }

  void OnDisable() {
    StopCoroutine("shootBullets");
  }

  IEnumerator shootBullets() {
    while(true) {
      yield return new WaitForSeconds(nonShootDuration);
      float distance = Vector3.Distance(player.transform.position, transform.position);
      if (distance > detectDistance) continue;

      for (int i = 0; i < numBullets; i++) {
        GameObject bullet = sam.getBullet(gunHead.transform.position);
        bullet.SetActive(true);
        yield return new WaitForSeconds(shootInterval);
      }
    }
  }
}
