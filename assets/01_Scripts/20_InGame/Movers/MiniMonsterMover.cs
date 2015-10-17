using UnityEngine;
using System.Collections;

public class MiniMonsterMover : ObjectsMover {
	private MonsterManager monm;
  private SpecialPartsManager spm;
  private float time;
  private bool timeElapsed = false;
  private float speed_chase;

  protected override void initializeRest() {
    monm = (MonsterManager)objectsManager;
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();

    if (player.isRidingMonster()) {
      time = monm.minimonStartTimeByPlayer;
    } else {
      time = monm.minimonStartTimeByMonster;
    }
    speed_chase = monm.speed + monm.minimonAdditionalSpeed;

    StartCoroutine("destroyByTime");
  }

  protected override float getSpeed() {
    return player.isRidingMonster()? monm.minimonStartSpeedByPlayer : monm.minimonStartSpeedByMonster;
  }

  protected override float getTumble() {
    return monm.minimonTumble;
  }

  protected override Vector3 getDirection() {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    return new Vector3(randomV.x, 0, randomV.y);
  }

  protected override void normalMovement() {
    if (time > 0) {
      time -= Time.deltaTime;
    } else {
      timeElapsed = true;

      direction = player.transform.position - transform.position;
      float distance = direction.magnitude;
      direction /= distance;

      if (distance > monm.detectDistance) {
        rb.velocity = direction * speed_chase * 2;
      } else if (isMagnetized) {
        rb.velocity = direction * player.getSpeed() * 1.5f;
      } else if (player.isRidingMonster() && player.getSpeed() != 0) {
        rb.velocity = direction * player.getSpeed() * 1.5f;
      } else {
        rb.velocity = direction * speed_chase;
      }
    }
  }

  IEnumerator destroyByTime() {
    yield return new WaitForSeconds(monm.minimonLifeTime);
    destroyObject();
  }

  public override void destroyObject(bool destroyEffect = true, bool byPlayer = false) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }

    if (destroyEffect) {
      Instantiate(monm.minimonDestroyEffect, transform.position, transform.rotation);
    }

    Destroy(gameObject);
  }

  override public string getManager() {
    return "MonsterManager";
  }

  override public bool dangerous() {
    if (player.isInvincible() || player.isUsingMagnet()) return false;
    else return true;
  }

  override public void encounterPlayer(bool destroy = true) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }

    Destroy(gameObject);
    Instantiate(monm.minimonDestroyEffect, transform.position, transform.rotation);
  }

  override public int cubesWhenEncounter() {
    return monm.cubesWhenDestroyMinimon;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (monm.cubesWhenDestroyMinimon * spm.bonus) : 0;
  }

  public bool isTimeElapsed() {
    return timeElapsed;
  }

  override public bool hasEncounterEffect() {
    return false;
  }

  override public void setMagnetized() {
    if (timeElapsed) isMagnetized = true;
  }
}
