using UnityEngine;
using System.Collections;

public class BigObstaclesMover : ObjectsMover {
  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  protected override int strength() {
    return 2;
  }

  public override void destroyObject() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    Destroy(gameObject);
  }
}
