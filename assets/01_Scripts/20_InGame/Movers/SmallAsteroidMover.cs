using UnityEngine;
using System.Collections;

public class SmallAsteroidMover : ObjectsMover {
private bool isNearPlayer = false;
  private SpecialPartsManager spm;
  // private SmallAsteroidManager ssm;
  private bool collideChecked = false;

  override public string getManager() {
    return "SmallAsteroidManager";
  }

  override protected void initializeRest() {
    canBeMagnetized = false;
    // ssm = (SmallAsteroidManager)objectsManager;
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();
  }

  override protected void afterDestroy(bool byPlayer) {
    if (isNearPlayer) player.nearAsteroid(false);
  }

  override protected void afterEncounter() {
    if (isNearPlayer) player.nearAsteroid(false);
  }

  public void nearPlayer(bool enter = true) {
    if (collideChecked == enter) return;
    else collideChecked = enter;

    isNearPlayer = enter;

    player.nearAsteroid(enter);
  }

  override public bool dangerous() {
    if (player.isInvincible()) return false;
    else return true;
  }

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }
}
