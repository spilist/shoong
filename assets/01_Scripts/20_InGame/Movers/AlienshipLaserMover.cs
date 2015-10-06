using UnityEngine;
using System.Collections;

public class AlienshipLaserMover : MonoBehaviour {
  AlienshipMover father;
  int targetRadius;
  float radius = 0;
  int targetLength;
  float length = 0;
  int rotatingSpeed;
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
    outer = transform.Find("Outer");
    this.father = father;

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
      else status++;
    } else if (status == 3) {
      radius = Mathf.MoveTowards(radius, 0, Time.deltaTime * targetRadius / shrinkingDuration);
      transform.localScale = new Vector3(length, radius, radius);
      if (radius == 0) {
        father.laserEnd();
        Destroy(gameObject);
      }
    }
	}

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      other.GetComponent<PlayerMover>().scoreManager.gameOver("Alienship");
      return;
    }

    if (other.tag == "Alienship") return;

    other.GetComponent<ObjectsMover>().destroyObject();
  }
}

