using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Panel : MonoBehaviour {
	public AutoBoosterButton abb;
  public string controlMethod;
  public float adjustScale;
  private bool LRMoving = false;
  private string movingDirection;

  void Start() {
    if (DataManager.dm.getString("ControlMethod") == controlMethod) {
      if (GetComponent<Image>() != null) GetComponent<Image>().enabled = true;
      if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = true;

      if (name == "StickPanel_movement") transform.Find("FingerIndicator").GetComponent<Image>().enabled = true;

    } else {
      if (controlMethod == "8Dir") gameObject.SetActive(false);
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

    if (tag == "StickPanel_booster") {
      Player.pl.crouch(true, "Stick");
      // Player.pl.shootBooster();
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

    if (tag == "StickPanel_booster") {
      Player.pl.crouch(false, "Stick");
    }
  }
}
