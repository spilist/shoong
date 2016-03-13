using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootAlienshipManager : ObjectsManager {
  public GameObject bulletPrefab;
  public List<GameObject> bulletPool;
  public GameObject bulletExplosionPrefab;
  public List<GameObject> bulletExplosionPool;
  public int bulletPoolAmount = 15;
  public int spawnRadius = 250;
  public int detectDistance = 200;
  public int headFollowingSpeed = 300;
  public float offScreenSpeedScale = 0.5f;

  public int bulletDamage = 40;
  public int numBullets = 3;
  public float shootInterval = 0.5f;
  public float nonShootDuration = 3;
  public float bulletSpeed = 160;
  public float bulletTumble = 50;
  public float firstSpawnDelay = 2;

  override public void initRest() {
    bulletPool = new List<GameObject>();
    for (int i = 0; i < bulletPoolAmount; ++i) {
      GameObject bullet = (GameObject) Instantiate(bulletPrefab);
      bullet.SetActive(false);
      bulletPool.Add(bullet);

      GameObject bulletExplosion = (GameObject) Instantiate(bulletExplosionPrefab);
      bulletExplosion.SetActive(false);
      bulletExplosionPool.Add(bulletExplosion);
    }
    Invoke("spawn", firstSpawnDelay);
  }

  public GameObject getBullet(Vector3 pos) {
    return getPooledObj(bulletPool, bulletPrefab, pos);
  }

  public GameObject getBulletExplosion(Vector3 pos) {
    return getPooledObj(bulletExplosionPool, bulletExplosionPrefab, pos);
  }

  override protected void spawn() {
    if (player == null || ScoreManager.sm.isGameOver()) return;

    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;
    Vector3 spawnPos = new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
    instance = getPooledObj(objPool, objPrefab, spawnPos);
    instance.transform.rotation = Quaternion.LookRotation(player.transform.position - spawnPos);
    instance.SetActive(true);
  }

  override protected float spawnInterval() {
    return Random.Range(minSpawnInterval, maxSpawnInterval);
  }
}
