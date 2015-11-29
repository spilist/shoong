using UnityEngine;
using System.Collections;

public class Skill_Shield : Skill {
  public ParticleSystem loseShieldEffect;

  override public void afterActivate(bool val) {
    if (!val) {
      Debug.Log("??");
      loseShieldEffect.Play();
      loseShieldEffect.GetComponent<AudioSource>().Play();
    }
  }

  override public bool hasDuration() {
    return true;
  }
}
