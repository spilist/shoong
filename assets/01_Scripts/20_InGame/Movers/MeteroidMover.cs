using UnityEngine;
using System.Collections;

public class MeteroidMover : ObjectsMover {
  public bool bigger = false;

  private SpecialPartsManager spm;
  private bool avoiding = false;
  private bool alreadyChecked = false;

  override public string getManager() {
    return "MeteroidManager";
  }

  protected override void initializeRest() {
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();

    canBeMagnetized = false;
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
    if (avoiding && !alreadyChecked && !player.isRidingMonster()) {
      player.showEffect("Whew");
    }
  }

  public void nearPlayer(bool enter = true) {
    avoiding = enter;

    if (!enter && !alreadyChecked) alreadyChecked = true;
  }

  public bool isAlreadyChecked() {
    return alreadyChecked;
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }
}
