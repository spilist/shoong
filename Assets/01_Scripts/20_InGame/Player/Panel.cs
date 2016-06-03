using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Panel : MonoBehaviour {
	// public AutoBoosterButton abb;
  public string controlMethod;
  public float adjustScale;
  public AbilityButton ability;
  private bool LRMoving = false;
  private string movingDirection;

  void Start() {
    if (DataManager.dm.getString("ControlMethod") != controlMethod) {
      gameObject.SetActive(false);
    }

    if (adjustScale > 0) {
      transform.localScale *= adjustScale / transform.lossyScale.x;
    }

    // if (tag == "StickPanel_booster" && abb != null && abb.isOn()) {
    //   GetComponent<Collider>().enabled = false;
    //   transform.Find("Touch").GetComponent<Text>().text = "AUTO";
    //   transform.Find("OnTheBeat").GetComponent<Text>().text = "MODE";
    // }
  }

  void Update() {
    if (Player.pl.uncontrollable()) return;

    if (LRMoving && Input.touchCount == 1) {
      Player.pl.setPerpDirection(movingDirection);
    }
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    // 고정스틱
    if (tag == "StickPanel_booster") {

    // if (Input.touchCount > 1 && tag == "StickPanel_booster") {
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
