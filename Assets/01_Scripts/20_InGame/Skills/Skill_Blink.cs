using UnityEngine;
using System.Collections;

public class Skill_Blink : Skill {
  public DoppleManager dpm;
  public int numContinuousBlinks = 4;
  public float intervalBetweenBlinks = 0.05f;
  private CharacterChangeManager cm;

  override public void afterStart() {
    dpm.enabled = true;
    cm = Player.pl.GetComponent<CharacterChangeManager>();
  }

	override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Blink");
      StartCoroutine("blink");
    } else {
      cm.changeCharacterToOriginal();
    }
  }

  IEnumerator blink() {
    int count = 0;

    while (count < numContinuousBlinks) {
      Player.pl.teleport();
      count++;
      yield return new WaitForSeconds(intervalBetweenBlinks);
    }

    cm.changeCharacterToOriginal();
  }
}
