using UnityEngine;
using System.Collections;

public class DangerousEMPField : MonoBehaviour {
  PlayerMover player;
  DangerousEMPManager dem;
  float maxScale;
  int rotatingSpeed;
  float enlargeDuration;
  float stayDuration;
  float shrinkDuration;

  float radius = 0;
  int status = 0;
  float stayCount = 0;

	void Start () {
    player = GameObject.Find("Player").GetComponent<PlayerMover>();

    dem = GameObject.Find("Field Objects").GetComponent<DangerousEMPManager>();
    maxScale = dem.empScale;
    rotatingSpeed = dem.empRotatingSpeed;
    enlargeDuration = dem.enlargeDuration;
    stayDuration = dem.stayDuration;
    shrinkDuration = dem.shrinkDuration;

    status = 1;
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
        Destroy(gameObject);
      }
    }
	}

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      if (!player.isInvincible()) {
        player.scoreManager.gameOver("EMP");
        return;
      }
    }
    if (other.GetComponent<ObjectsMover>() == null) Debug.LogWarning("Null mover: " + other.tag);
    else other.GetComponent<ObjectsMover>().destroyObject();
  }
}
