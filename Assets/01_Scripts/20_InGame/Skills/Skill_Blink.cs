using UnityEngine;
using System.Collections;

public class Skill_Blink : Skill {
  public DoppleManager dpm;

  override public void afterStart() {
    dpm.enabled = true;
  }

	override public void afterActivate(bool val) {
    Camera.main.GetComponent<CameraMover>().setSlowly(val);

    if (!val) Player.pl.afterStrengthenStart();
  }
}
