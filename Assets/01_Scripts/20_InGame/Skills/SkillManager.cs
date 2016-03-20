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
    if (equiped != null) equiped.activate(false);
  }

  public void equip(string skillName) {
    if (skillName == "None") {
      equiped = null;
    } else {
      equiped = getSkill(skillName);
      RhythmManager.rm.setLoop(equiped.normalRing, equiped.skillRing);
    }
  }

  public void activate() {
    if (!equiped.isActivated()) {
      equiped.activate(true);
      RhythmManager.rm.loopSkillActivated(true);
    }
  }

  public bool skillRunning() {
    if (equiped == null) return false;
    return equiped.isActivated();
  }

  public bool isBlink() {
    if (equiped == null) return false;
    return equiped.name == "Blink";
  }
  
  public bool isFever() {
    if (equiped == null) return false;
    return equiped.name == "Fever";
  }

  public bool isInfiniteBooster() {
    if (equiped == null) return false;
    return (equiped.name == "Fever" || equiped.name == "Solar");
  }

  public Skill current() {
    return equiped;
  }

  public string skillName() {
    if (equiped == null) return "None";
    else return equiped.name;
  }
}
