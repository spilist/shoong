using UnityEngine;
using System.Collections;

public class Skill_Fever : Skill {
  override public void afterActivate(bool val) {
    RhythmManager.rm.setFever(val);
  }
}
