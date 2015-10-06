using UnityEngine;
using System.Collections;

public class AlienshipMover : ObjectsMover {
  private AlienshipManager asm;
  private int shootLaserPer;
  private float chargeTime;
  private int headFollowingSpeed;
  private GameObject laserCanon;
  private GameObject laserPrefab;
  private GameObject laserWarningPrefab;

  private GameObject laserWarning;

  Quaternion rotation;
  float angleY;

  int shootingStatus = 0;
  float stayCount = 0;

  protected override void initializeRest() {
    asm = (AlienshipManager)objectsManager;
    canBeMagnetized = false;

    shootLaserPer = asm.shootLaserPer;
    chargeTime = asm.chargeTime;
    headFollowingSpeed = asm.headFollowingSpeed;

    laserCanon = transform.Find("LaserCanon").gameObject;
    laserPrefab = asm.laserPrefab;
    laserWarningPrefab = asm.laserWarningLinePrefab;
    laserWarningPrefab.transform.localScale = new Vector3(asm.laserLength, asm.laserRadius, asm.laserRadius);
    angleY = transform.eulerAngles.y;
    shootingStatus = 1;
  }

  protected override void normalMovement() {
    rb.velocity = direction * getSpeed();
    rb.angularVelocity = Vector3.zero;
  }

  override public string getManager() {
    return "AlienshipManager";
  }

  override public bool dangerous() {
    return true;
  }

  override protected float getTumble() {
    return 0;
  }

  void Update() {
    if (shootingStatus == 1) {
      direction = getDirection();
      rotation = Quaternion.LookRotation(direction);
      float targetAngle = rotation.eulerAngles.y;
      if (Mathf.Abs(targetAngle - angleY) > 180) targetAngle -= 360;
      angleY = Mathf.MoveTowards(angleY, targetAngle, Time.deltaTime * headFollowingSpeed);
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, transform.eulerAngles.z);

      transform.Rotate(0, 0, Time.deltaTime * asm.tumble);

      if (stayCount < shootLaserPer) {
        if (player.isUsingEMP()) return;
        stayCount += Time.deltaTime;
      } else {
        stayCount = 0;
        shootingStatus++;
        rb.isKinematic = true;
        laserCanon.SetActive(true);
        laserWarning = (GameObject) Instantiate(laserWarningPrefab, laserCanon.transform.position, Quaternion.identity);
        laserWarning.transform.eulerAngles = new Vector3(0, angleY - 90, 0);
      }
    } else if (shootingStatus == 2) {
      if (stayCount < chargeTime) {
        if (player.isUsingEMP()) return;
        stayCount += Time.deltaTime;
      } else {
        stayCount = 0;
        AlienshipLaserMover laser = ((GameObject)Instantiate(laserPrefab, laserWarning.transform.position, Quaternion.identity)).GetComponent<AlienshipLaserMover>();
        laser.set(laserWarning.transform.eulerAngles.y, asm, this);
        Destroy(laserWarning);
        shootingStatus = 0;
      }
    }
  }

  public void laserEnd() {
    shootingStatus = 1;
    rb.isKinematic = false;
    laserCanon.SetActive(false);
  }
}
