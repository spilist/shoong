using UnityEngine;
using System.Collections;

public class PlayerLaser : MonoBehaviour {
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

	void Start() {
    Skill_laser skill = SkillManager.sm.current().GetComponent<Skill_laser>();

    targetRadius = skill.laserRadius;
    targetLength = skill.laserLength;
    shootingDuration = skill.laserShootingDuration;
    stayDuration = skill.laserStayDuration;
    shrinkingDuration = skill.laserShrinkingDuration;
    rotatingSpeed = skill.laserRotatingSpeed;
    outer = transform.Find("Outer");
  }

  public void set(float angle) {
    transform.eulerAngles = new Vector3(0, angle, 0);

    stayCount = 0;
    radius = 0;
    transform.localScale = Vector3.zero;

    status = 1;
  }

  float playerAngle() {
    return Quaternion.LookRotation(Player.pl.getDirection()).eulerAngles.y - 90;
  }

  void Update () {
    transform.eulerAngles = new Vector3(0, playerAngle(), 0);

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
        SkillManager.sm.stopSkills();

        gameObject.SetActive(false);
      }
    }
  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();

    if (mover == null || other.tag == "Player" || other.tag == "Blackhole") return;

    Player.pl.goodPartsEncounter(mover, mover.cubesWhenDestroy(), other.tag == "GoldenCube");
  }
}
