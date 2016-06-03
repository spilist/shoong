using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityButton : MenusBehavior {
  public float coolDown_unstoppable = 2;
  public float duration_unstoppable = 0.3f;
  public float speedup_unstoppable = 250;
  public float coolDown_escape = 1;
  public float duration_escape = 0.2f;
  public float speedup_escape = 300;

  private string abilityName;
  private float coolDown;
  private float duration;
  private float speedup;
  private bool available = true;

	void Start () {
    abilityName = DataManager.dm.getString("DashMode");
    if (abilityName == "Unstoppable") {
      coolDown = coolDown_unstoppable;
      duration = duration_unstoppable;
      speedup = speedup_unstoppable;
    } else {
      coolDown = coolDown_escape;
      duration = duration_escape;
      speedup = speedup_escape;
    }
	}

	override public void activateSelf() {
    if (available) {
      playTouchSound = false;

      if (abilityName == "Unstoppable") {
        Player.pl.dash(duration, speedup, false);
      } else {
        Player.pl.dash(duration, speedup, true);
      }

      available = false;
      Invoke("enableAbility", coolDown);
    }
  }

  void enableAbility() {
    available = true;
    playTouchSound = true;
  }

  void OnPointerDown() {
    activateSelf();
  }
}
