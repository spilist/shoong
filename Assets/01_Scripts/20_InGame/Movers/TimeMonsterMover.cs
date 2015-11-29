using UnityEngine;
using System.Collections;

public class TimeMonsterMover : ObjectsMover {
  TimeMonsterManager tmm;
  float speedScale;

  override public string getManager() {
    return "TimeMonsterManager";
  }

  protected override void initializeRest() {
    tmm = (TimeMonsterManager)objectsManager;
    speedScale = tmm.speedScale;
    canBeMagnetized = false;
  }

  protected override void normalMovement() {
    direction = player.transform.position - transform.position;
    direction = direction / direction.magnitude;
    rb.velocity = direction * getSpeed();
  }

  override protected float getSpeed() {
    if (player.isUsingEMP()) {
      return 0;
    } else if (Vector3.Distance(player.transform.position, transform.position) < 10) {
      return player.getSpeed();
    } else {
      return player.getSpeed() * speedScale;
    }
  }

  override public bool dangerous() {
    return true;
  }
}
