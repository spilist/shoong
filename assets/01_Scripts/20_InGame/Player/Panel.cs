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
    if (stickMoving && Input.touchCount == 1) {
      handler.setPlayerDirection(character);
    }
  }

  void OnPointerDown() {
    if (tag == "StickPanel_movement") {
      stickMoving = true;
    }

    if (tag == "StickPanel_booster") {
      Player.pl.shootBooster();
    }
  }

  void OnPointerUp() {
    stickMoving = false;
  }
}
