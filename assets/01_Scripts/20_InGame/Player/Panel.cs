using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {
	public string controlMethod;
  public TouchInputHandler handler;
  public Transform character;
  private bool stickMoving = false;

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
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    if (tag == "StickPanel_movement") {
      stickMoving = true;
    }

    if (Input.touchCount > 1 && tag == "StickPanel_booster") {
      Player.pl.shootBooster();
    }
  }

  void OnPointerUp() {
    stickMoving = false;
  }
}
