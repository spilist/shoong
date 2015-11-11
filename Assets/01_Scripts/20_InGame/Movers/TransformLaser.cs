using UnityEngine;
using System.Collections;

public class TransformLaser : MonoBehaviour {
  float duration;
  int status = 0;
  float targetAngle;
  Vector3 direction;
  float targetLength = 0;
  float length = 0;

	public void shoot(Vector3 target, float duration) {
    direction = target - transform.position;
    targetAngle = Quaternion.LookRotation(direction).eulerAngles.y - 90;
    transform.eulerAngles = new Vector3(0, targetAngle, 0);
    targetLength = Vector3.Distance(target, transform.position) / 2f;

    this.duration = duration;
    status = 1;
    length = 0;
  }

  void Update() {
    if (status == 1) {
      length = Mathf.MoveTowards(length, targetLength, Time.deltaTime * targetLength / duration);
      transform.localScale = new Vector3(length, transform.localScale.y, transform.localScale.z);
      if (length == targetLength) {
        status++;
        gameObject.SetActive(false);
      }
    }
  }
}
