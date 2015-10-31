using UnityEngine;
using System.Collections;

public class DangerousEMPField : MonoBehaviour {
  DangerousEMPManager dem;
  float maxScale;
  int rotatingSpeed;
  float enlargeDuration;
  float stayDuration;
  float shrinkDuration;

  float radius = 0;
  int status = 0;
  float stayCount = 0;

	void Awake () {
    dem = GameObject.Find("Field Objects").GetComponent<DangerousEMPManager>();
    rotatingSpeed = dem.empRotatingSpeed;
    enlargeDuration = dem.enlargeDuration;
    stayDuration = dem.stayDuration;
    shrinkDuration = dem.shrinkDuration;
  }

  void OnEnable() {
    radius = 0;
    stayCount = 0;
    status = 1;
    maxScale = dem.empScale;
  }

	void Update () {
    transform.Rotate(-Vector3.up * Time.deltaTime * rotatingSpeed);

    if (status == 1) {
      radius = Mathf.MoveTowards(radius, maxScale, Time.deltaTime * maxScale / enlargeDuration);

      transform.localScale = radius * Vector3.one;
      transform.Find("Halo").GetComponent<Light>().range = radius;

      if (radius == maxScale) status = 2;
    } else if (status == 2) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        status = 3;
      }
    } else if (status == 3) {
      radius = Mathf.MoveTowards(radius, 0, Time.deltaTime * maxScale / shrinkDuration);

      transform.localScale = radius * Vector3.one;
      transform.Find("Halo").GetComponent<Light>().range = radius;

      if (radius == 0) {
        status = 0;
        gameObject.SetActive(false);
      }
    }
	}

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      if (!Player.pl.isInvincible()) {
        Player.pl.loseEnergy(dem.loseEnergy(), "DangerousEMPField");
        return;
      }
    }
    if (other.GetComponent<ObjectsMover>() == null) Debug.LogWarning("Null mover: " + other.tag);
    else other.GetComponent<ObjectsMover>().destroyObject();
  }
}
