﻿using UnityEngine;
using System.Collections;

public class MonsterMover : ObjectsMover {
  private float speed_chase;
  private float speed_runaway;
  private float speed_weaken;

  private MonsterManager monm;

  private float originalScale;
  private bool weak = false;
  private bool shrinking = true;

  public ParticleSystem aura;
  public ParticleSystem weakenAura;

  protected override void initializeRest() {
    monm = GameObject.Find("Field Objects").GetComponent<MonsterManager>();
    originalScale = shrinkedScale;
    speed_chase = monm.speed_chase;
    speed_runaway = monm.speed_runaway;
    speed_weaken = monm.speed_weaken;
    StartCoroutine("weakened");
  }

  protected override float getSpeed() {
    return speed_chase;
  }

  protected override float getTumble() {
    return monm.tumble;
  }

  protected override Vector3 getDirection() {
    Vector3 dir = player.transform.position - transform.position;
    return dir / dir.magnitude;
  }

  IEnumerator weakened() {
    yield return new WaitForSeconds(Random.Range(monm.minLifeTime, monm.maxLifeTime));
    weak = true;
    monm.stopWarning();
    weakenAura.Play();
    aura.Stop();
    GetComponent<Renderer>().material.SetColor("_OutlineColor", monm.weakenedOutlineColor);

    yield return new WaitForSeconds(monm.weakenDuration);
    destroyObject();
  }

  public bool isWeak() {
    return weak;
  }

  protected override int strength() {
    return 3;
  }

  protected override void normalMovement() {
    direction = player.transform.position - transform.position;
    float distance = direction.magnitude;
    direction /= distance;
    if (distance > monm.detectDistance){
      GetComponent<Rigidbody>().velocity = direction * speed_chase * 2;
    } else if (isMagnetized) {
      GetComponent<Rigidbody>().velocity = direction * player.GetComponent<Rigidbody>().velocity.magnitude * fom.unstoppableFollowSpeed;
    } else if (weak) {
      GetComponent<Rigidbody>().velocity = -direction * speed_weaken;
    } else if (player.GetComponent<PlayerMover>().isUnstoppable()) {
      GetComponent<Rigidbody>().velocity = -direction * speed_runaway;
    } else {
      GetComponent<Rigidbody>().velocity = direction * speed_chase;
    }

    if (weak) {
      if (shrinking) {
        shrinkedScale = Mathf.MoveTowards(shrinkedScale, monm.shrinkUntil, Time.deltaTime * monm.shrinkSpeed);
        if (shrinkedScale == monm.shrinkUntil) shrinking = false;
      } else {
        shrinkedScale = Mathf.MoveTowards(shrinkedScale, originalScale, Time.deltaTime * monm.restoreSpeed);
        if (shrinkedScale == originalScale) shrinking = true;
      }
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    }
  }

  public override void destroyObject() {
    Destroy(gameObject);
    monm.stopWarning();
    Instantiate(monm.destroyEffect, transform.position, transform.rotation);
    monm.run();
  }

  override public void encounterPlayer() {
    Destroy(gameObject);
    monm.stopWarning();
    Instantiate(monm.destroyEffect, transform.position, transform.rotation);
    monm.run();
  }
}
