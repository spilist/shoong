using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
  public static SkillManager sm;
  private Skill equiped;

  void Awake() {
    sm = this;
  }

  public Skill getSkill(string name) {
    return transform.Find(name).GetComponent<Skill>();
  }

  public void stopSkills() {
    equiped.activate(false);
  }

  public void equip(string skillName) {
    equiped = getSkill(skillName);
    RhythmManager.rm.setLoop(equiped.normalRing, equiped.skillRing);
  }

  public void activate() {
    equiped.activate(true);
  }
}
