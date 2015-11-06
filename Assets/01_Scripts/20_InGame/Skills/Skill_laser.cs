using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_laser : Skill {
  public Transform followPlayer;
  public GameObject laserPrefab;
  private List<GameObject> laserPool;
  public int laserAmount = 2;
  public float chargeTime = 0.5f;
  public int laserRadius = 20;
  public int laserLength = 500;
  public float laserShootingDuration = 0.5f;
  public float laserStayDuration = 1;
  public float laserShrinkingDuration = 0.3f;
  public int laserRotatingSpeed = 1000;
  public float pointsLaserGetScale = 0.5f;

  Quaternion rot;

	override public void afterStart() {
    laserPool = new List<GameObject>();
    for (int i = 0; i < laserAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(laserPrefab);
      obj.SetActive(false);
      obj.transform.SetParent(followPlayer, false);
      laserPool.Add(obj);
    }
  }

  public GameObject getLaser() {
    return getPooledObj(laserPool, laserPrefab, followPlayer);
  }

  override public void afterActivate(bool val) {
    if (val) {
      rot = Quaternion.LookRotation(Player.pl.getDirection());

      GameObject laser = getLaser();
      laser.SetActive(true);
      laser.GetComponent<PlayerLaser>().set(rot.eulerAngles.y - 90);
    }
  }
}
