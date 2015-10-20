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
    if (stickMoving) {
      handler.setPlayerDirection(character);
    }
  }

  void OnMouseDown() {
    if (tag == "StickPanel_movement") {
      if (Input.touchCount > 0) {
        stickMoving = true;
      }
    }
  }

  void OnMouseUp() {
    stickMoving = false;
  }
}
