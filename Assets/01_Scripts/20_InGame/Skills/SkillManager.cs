using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillManager : MonoBehaviour {
  public static SkillManager sm;
  public Text smashText;
  private Skill equipped;
  private int skillCooldown;

  void Awake() {
    sm = this;
  }

  public Skill getSkill(string name) {
    return transform.Find(name).GetComponent<Skill>();
  }

  public void stopSkills() {
    if (equipped != null) equipped.activate(false);
  }

  public void equip(string skillName) {
    if (skillName == "None") {
      equipped = null;
      skillCooldown = 0;
    } else {
      equipped = getSkill(skillName);
      RhythmManager.rm.setLoop(equipped.normalRing, equipped.skillRing);
      skillCooldown = equipped.dashCooldown;
      setSmashText();
    }
  }

  public void activate() {
    if (!equipped.isActivated()) {
      equipped.activate(true);
      RhythmManager.rm.loopSkillActivated(true);
    }
  }

  public bool skillRunning() {
    if (equipped == null) return false;
    return equipped.isActivated();
  }

  public bool isBlink() {
    if (equipped == null) return false;
    return equipped.name == "Blink";
  }

  public bool isFever() {
    if (equipped == null) return false;
    return equipped.name == "Fever";
  }

  public bool isInfiniteBooster() {
    if (equipped == null) return false;
    return (equipped.name == "Fever" || equipped.name == "Solar");
  }

  public Skill current() {
    return equipped;
  }

  public string skillName() {
    if (equipped == null) return "None";
    else return equipped.name;
  }

  public void activateWithDash() {
    if (equipped == null) return;

    if (skillCooldown == 0) {
      activate();
      skillCooldown = equipped.dashCooldown;
    } else {
      skillCooldown--;
    }

    setSmashText();
  }

  void setSmashText() {
    if (skillCooldown == 0) {
      smashText.text = "SUPER\nSMASH!!";
    } else {
      smashText.text = "SMASH!!";
    }
  }

  public bool skillAvailable() {
    return skillCooldown == 0;
  }
}
