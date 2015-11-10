using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {
	public string controlMethod;
  public float adjustScale;
  private bool LRMoving = false;
  private string movingDirection;

  void Start() {
    if (DataManager.dm.getString("ControlMethod") != controlMethod) {
      gameObject.SetActive(false);
    }

    if (adjustScale > 0) {
      transform.localScale *= adjustScale / transform.lossyScale.x;
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
    if (tag == "LRPanel_left" || tag == "LRPanel_right") {
      LRMoving = false;
    }
  }
}
