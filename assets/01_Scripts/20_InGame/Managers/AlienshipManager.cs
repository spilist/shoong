using UnityEngine;
using System.Collections;

public class AlienshipManager : ObjectsManager {
  public GameObject laserPrefab;

  public int speedIncreaseAmount = 10;
  public float speedIncreasePer = 5;
  public int shootLaserPer = 10;
  public float chargeTime = 0.5f;
  public int laserSpeed = 250;
  public float laserSpeedIncreaseAmount = 5;

  override public void initRest() {
    skipInterval = true;
  }

  override public Vector3 getDirection() {
    Vector3 dir = player.transform.position - instance.transform.position;
    return dir / dir.magnitude;
  }
}
