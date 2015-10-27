using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {
	public string controlMethod;
  private bool LRMoving = false;
  private string movingDirection;

  void Start() {
    if (DataManager.dm.getString("ControlMethod") != controlMethod) {
      gameObject.SetActive(false);
    }
  }

  void Update() {
    if (Player.pl.uncontrollable()) return;

    if (LRMoving && Input.touchCount == 1) {
      Player.pl.setPerpDirection(movingDirection);
    }
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    if (Input.touchCount == 1) {
      LRMoving = true;
      movingDirection = tag;
    }
  }

  void OnPointerUp() {
    LRMoving = false;
    Player.pl.playerShip.tiltBack();
  }
}
