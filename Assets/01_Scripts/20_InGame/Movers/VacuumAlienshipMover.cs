using UnityEngine;
using System.Collections;

public class VacuumAlienshipMover : ObjectsMover {
  private VacuumAlienshipManager vam;
  private int headFollowingSpeed;
  private float offScreenSpeedScale;
  private int detectDistance;

  Quaternion rotation;
  float angleY;

	protected override void initializeRest() {
    vam = (VacuumAlienshipManager)objectsManager;
    canBeMagnetized = false;

    headFollowingSpeed = vam.headFollowingSpeed;
    offScreenSpeedScale = vam.offScreenSpeedScale;
    speed = vam.speed;
    detectDistance = vam.detectDistance;
  }

  protected override void afterEnable() {
    angleY = transform.eulerAngles.y;
    speed = vam.speed;
  }

  protected override void normalMovement() {
    rb.velocity = direction * getSpeed();
    rb.angularVelocity = Vector3.zero;
  }

  override public string getManager() {
    return "VacuumAlienshipManager";
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

  override protected void afterCollidePlayer(bool effect) {
    ScoreManager.sm.gameOver("VacuumAlienship");
  }

  override protected void afterEncounter() {
    base.afterEncounter();
    vam.run();
  }

  void Update() {
    direction = getDirection();
    rotation = Quaternion.LookRotation(direction);
    float targetAngle = rotation.eulerAngles.y;
    if (Mathf.Abs(targetAngle - angleY) > 180) targetAngle -= 360;
    angleY = Mathf.MoveTowards(angleY, targetAngle, Time.deltaTime * headFollowingSpeed);
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, transform.eulerAngles.z);

    transform.Rotate(0, 0, Time.deltaTime * vam.tumble);
  }

}
