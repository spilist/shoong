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
    Vector3 dir = player.transform.position - transform.position;
    return dir / dir.magnitude;
  }

  protected override int strength() {
    return 1;
  }

  public override void destroyObject() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    Destroy(gameObject);
  }

  public override void encounterPlayer() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    Destroy(gameObject);
  }
}
