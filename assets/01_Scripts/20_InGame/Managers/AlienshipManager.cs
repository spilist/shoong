using UnityEngine;
using System.Collections;

public class AlienshipManager : ObjectsManager {
  public GameObject laserPrefab;
  public GameObject laserWarningLinePrefab;

  public int spawnRadius = 200;
  public int headFollowingSpeed = 100;
  public int speedIncreaseAmount = 10;
  public float speedIncreasePer = 5;
  public int shootLaserPer = 10;
  public float chargeTime = 0.5f;

  public int laserRadius = 20;
  public int laserLength = 500;
  public float laserShootingDuration = 0.5f;
  public float laserStayDuration = 1;
  public float laserShrinkingDuration = 0.3f;
  public int laserRotatingSpeed = 1000;
  private int timeSpawned;

  override public void initRest() {
    skipInterval = true;
    run();
  }

  override public Vector3 getDirection() {
    Vector3 dir = player.transform.position - instance.transform.position;
    return dir / dir.magnitude;
  }

  override public float getSpeed() {
    if (player.isUsingEMP()) {
      return 0;
    } else if (Vector3.Distance(player.transform.position, instance.transform.position) < 10) {
      return player.GetComponent<Rigidbody>().velocity.magnitude;
    }
    else {
      int timeUnit = (int) Mathf.Floor((ElapsedTime.time.now - timeSpawned) / speedIncreasePer);
      return (speed + timeUnit * speedIncreaseAmount);
    }
  }

  override protected void spawn() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = (GameObject) Instantiate(objPrefab, spawnPos, Quaternion.LookRotation(player.transform.position - spawnPos));
    instance.transform.parent = transform;
  }

  override protected void afterSpawn() {
    timeSpawned = ElapsedTime.time.now;
  }
}
