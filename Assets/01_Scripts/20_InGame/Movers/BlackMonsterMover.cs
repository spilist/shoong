using UnityEngine;
using System.Collections;

public class BlackMonsterMover : ObjectsMover {
  private BlackMonsterManager bmm;
  private float slowStayDuration;
  private float increaseSpeedDuration;
  private int increaseSpeedUntil;
  private float chargeDuration;
  private float rushDuration;
  private float rushSpeed;
  private int detectDistance;
  private float offScreenSpeedScale;
  private Animation beatAnimation;

  private float stayCount = 0;

  override public string getManager() {
    return "BlackMonsterManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
    bmm = (BlackMonsterManager)objectsManager;
    slowStayDuration = bmm.slowStayDuration;
    increaseSpeedDuration = bmm.increaseSpeedDuration;
    increaseSpeedUntil = bmm.increaseSpeedUntil;
    detectDistance = bmm.detectDistance;
    offScreenSpeedScale = bmm.offScreenSpeedScale;
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  protected override void afterEnable() {
    stayCount = 0;
  }

  protected override void normalMovement() {
    Vector3 dir = player.transform.position - transform.position;
    direction = dir / dir.magnitude;
    if (dir.magnitude > detectDistance) {
      stayCount = 0;
      speed = bmm.speed + player.getSpeed() * offScreenSpeedScale;
    } else {
      if (stayCount < slowStayDuration) {
        stayCount += Time.fixedDeltaTime;
        speed = bmm.speed;
      } else if (stayCount < slowStayDuration + increaseSpeedDuration) {
        stayCount += Time.fixedDeltaTime;
        speed = Mathf.MoveTowards(speed, increaseSpeedUntil, Time.fixedDeltaTime * (increaseSpeedUntil - bmm.speed) / increaseSpeedDuration);
      } else {
        stayCount = 0;
        speed = bmm.speed;
      }
    }
    rb.velocity = direction * speed;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterCollidePlayer() {
    bmm.blindPlayer();
    destroyObject();
  }

  override protected void afterEncounter() {
    base.afterEncounter();
    if (bmm.enabled) bmm.run();
  }
}
