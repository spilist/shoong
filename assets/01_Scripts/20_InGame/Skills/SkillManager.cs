using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
  public static SkillManager sm;
  public SkillButton[] slots;
  public Skill[] skills;

  void Awake() {
    sm = this;
  }

  public void startGame() {
    string mainObjectsString = PlayerPrefs.GetString("MainObjects").Trim();
    if (mainObjectsString != "") {
      int count = 0;
      foreach (Skill skill in skills) {
        if (count >= slots.Length) return;
        if (mainObjectsString.Contains(skill.name)) {
          equip(skill, count++);
        }
      }
    }
  }

  void equip(Skill skill, int slotNumber) {
    skill.gameObject.SetActive(true);
    SkillButton btn = slots[slotNumber];
    btn.setSkill(skill);
  }

  public Skill getSkill(string name) {
    foreach (Skill skill in skills) {
      if (skill.name == name) return skill;
    }
    return null;
  }
}
