using UnityEngine;
using System.Collections;

public class ObstaclesMover : ObjectsMover {
  ObstaclesManager obm;

  protected override void initializeRest() {
    obm = GameObject.Find("Field Objects").GetComponent<ObstaclesManager>();
    canBeMagnetized = false;
  }

  protected override float getSpeed() {
    return obm.speed;
  }

  protected override float getTumble() {
    return obm.tumble;
  }

  protected override Vector3 getDirection() {
    return obm.getDirection();
  }

  protected override float strength() {
    return obm.strength;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);
  }

  public override void encounterPlayer() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    Destroy(gameObject);
  }
}
