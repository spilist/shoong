using UnityEngine;
using System.Collections;

public class PhaseMonsterMover : ObjectsMover {
  private PhaseMonsterManager pmm;
  private int speed_runaway;
  private int detectDistance;

	override public string getManager() {
    return "PhaseMonsterManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
    pmm = (PhaseMonsterManager)objectsManager;
    speed_runaway = pmm.speed_runaway;
    detectDistance = pmm.detectDistance;
  }

  protected override void normalMovement() {
    Vector3 dir = player.transform.position - transform.position;
    direction = dir / dir.magnitude;
    if (dir.magnitude > detectDistance) {
      rb.velocity = direction * speed * 2;
    } else if (player.isUnstoppable() || player.isUsingDopple() || player.isRidingMonster()) {
      rb.velocity = -direction * speed_runaway;
    } else {
      rb.velocity = direction * speed;
    }
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override protected void afterEncounter() {
    pmm.run();
  }
}
