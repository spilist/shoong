using UnityEngine;
using System.Collections;

public class AlienshipBullet : MonoBehaviour {
  ShootAlienshipManager sam;
  float speed;
  float tumble;
  int damage;
  Player player;
  Rigidbody rb;

	void Awake() {
    sam = GameObject.Find("Field Objects").GetComponent<ShootAlienshipManager>();
    speed = sam.bulletSpeed;
    tumble = sam.bulletTumble;
    damage = sam.bulletDamage;
    player = Player.pl;
    rb = GetComponent<Rigidbody>();
  }

  void OnEnable() {
    rb.angularVelocity = Random.onUnitSphere * tumble;
    rb.velocity = getDirection() * speed;
  }

  Vector3 getDirection() {
    Vector3 direction = player.transform.position - transform.position;
    return direction.normalized;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player" && !Player.pl.isInvincible()) {
      Player.pl.loseEnergy(damage, "Alienship");
      sam.getBulletExplosion(transform.position).SetActive(true);
      gameObject.SetActive(false);
      return;
    }

    ObjectsMover mover = other.GetComponent<ObjectsMover>();

    if (mover == null) return;

    if (mover.tag != "Blackhole") mover.destroyObject();
  }
}
