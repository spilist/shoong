using UnityEngine;
using System.Collections;

public class AlienshipLaserMover : MonoBehaviour {
  AlienshipMover father;
  int targetRadius;
  float radius = 0;
  int targetLength;
  float length = 0;
  int rotatingSpeed;
  int loseEnergy;
  Transform outer;

  float shootingDuration;
  float stayDuration;
  float shrinkingDuration;

  float stayCount = 0;
  int status = 0;

  public void set(float angle, AlienshipManager asm, AlienshipMover father) {
    transform.eulerAngles = new Vector3(0, angle, 0);
    targetRadius = asm.laserRadius;
    targetLength = asm.laserLength;
    shootingDuration = asm.laserShootingDuration;
    stayDuration = asm.laserStayDuration;
    shrinkingDuration = asm.laserShrinkingDuration;
    rotatingSpeed = asm.laserRotatingSpeed;
    loseEnergy = asm.laserLoseEnergy;
    outer = transform.Find("Outer");
    this.father = father;

    stayCount = 0;
    radius = 0;
    transform.localScale = Vector3.zero;

    status = 1;
  }

	void Update () {
    if (status == 1) {
      radius = Mathf.MoveTowards(radius, targetRadius, Time.deltaTime * targetRadius / shootingDuration);
      length = Mathf.MoveTowards(length, targetLength, Time.deltaTime * targetLength / shootingDuration);
      transform.localScale = new Vector3(length, radius, radius);
      if (radius == targetRadius) status++;
    } else if (status == 2) {
      outer.transform.localEulerAngles += new Vector3(Time.deltaTime * rotatingSpeed, 0, 0);

      if (stayCount < stayDuration) stayCount += Time.deltaTime;
      else {
        outer.transform.localEulerAngles = new Vector3(0, 0, 90);
        status++;
      }
    } else if (status == 3) {
      radius = Mathf.MoveTowards(radius, 0, Time.deltaTime * targetRadius / shrinkingDuration);
      transform.localScale = new Vector3(length, radius, radius);
      if (radius == 0) {
        father.laserEnd();
        gameObject.SetActive(false);
      }
    }
	}

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      Player.pl.loseEnergy(loseEnergy, "Alienship");
      return;
    }

    if (other.tag == "Alienship") return;

    ObjectsMover mover = other.GetComponent<ObjectsMover>();

    if (mover != null) mover.destroyObject();
  }
}

