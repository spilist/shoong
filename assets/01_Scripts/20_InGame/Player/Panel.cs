using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {
	public string controlMethod;
  public TouchInputHandler handler;
  public Transform character;
  private bool stickMoving = false;
  private bool LRMoving = false;
  private string movingDirection;

  void Start() {
    if (DataManager.dm.getString("ControlMethod") != controlMethod) {
      gameObject.SetActive(false);
    }
  }

  void Update() {
    if (Player.pl.uncontrollable()) return;

    if (stickMoving && Input.touchCount == 1) {
      handler.setPlayerDirection(character);
    }

    if (LRMoving && Input.touchCount == 1) {
      Player.pl.setPerpDirection(movingDirection);
    }
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    if (tag == "StickPanel_movement") {
      stickMoving = true;
    }

    if (Input.touchCount > 1 && tag == "StickPanel_booster") {
      Player.pl.shootBooster();
    }

    if (tag == "LRPanel_left" || tag == "LRPanel_right") {
      if (Input.touchCount == 1) {
        LRMoving = true;
        movingDirection = tag;
      } else {
        Player.pl.shootBooster();
      }
    }
  }

  void OnPointerUp() {
    stickMoving = false;
    LRMoving = false;
  }
}
