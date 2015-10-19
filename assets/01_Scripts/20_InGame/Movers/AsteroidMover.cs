using UnityEngine;
using System.Collections;

public class AsteroidMover : ObjectsMover {
  private bool isNearPlayer = false;
  private bool collideChecked = false;

  override public string getManager() {
    return "AsteroidManager";
  }

  override protected void initializeRest() {
    canBeMagnetized = false;
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
}
