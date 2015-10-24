﻿using UnityEngine;
using System.Collections;

public class MeteroidMover : ObjectsMover {
  public bool bigger = false;

  private bool avoiding = false;
  private bool alreadyChecked = false;

  override public string getManager() {
    return "MeteroidManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
  }

  override protected void afterEnable() {
    avoiding = false;
    alreadyChecked = false;
  }

  override protected float strength() {
    if (bigger) return ((MeteroidManager)objectsManager).biggerMeteroidStrength;
    else return objectsManager.strength;
  }

  override protected float getSpeed() {
    if (bigger) return ((MeteroidManager)objectsManager).biggerMeteroidSpeed;
    else return objectsManager.getSpeed();
  }

  override protected float getTumble() {
    if (bigger) return ((MeteroidManager)objectsManager).biggerMeteroidTumble;
    else return objectsManager.getTumble();
  }

  override public int cubesWhenEncounter() {
    if (bigger) return objectsManager.cubesWhenEncounter() * 2;
    else return objectsManager.cubesWhenEncounter();
  }

  override protected void afterCollide(Collision collision) {
    if (collision.collider.tag == tag) {
      if (bigger == collision.collider.GetComponent<MeteroidMover>().bigger) {
        destroyObject();
        return;
      }
    }
  }

  override protected void afterDestroy(bool byPlayer) {
    if (!byPlayer && avoiding && !alreadyChecked && !player.isRidingMonster()) {
      player.showEffect("Whew");
    }
  }

  override protected void afterCollidePlayer() {
    destroyObject();
  }

  public void nearPlayer(bool enter = true) {
    avoiding = enter;

    if (!avoiding && !alreadyChecked) alreadyChecked = true;
  }

  public bool isAlreadyChecked() {
    return alreadyChecked;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }
}
