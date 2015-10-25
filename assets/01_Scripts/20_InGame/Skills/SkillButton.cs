using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillButton : MonoBehaviour {
  private Skill skill;
  private string what;
  private float duration;
  private float coolTime;

  private Collider sCollider;
  private Image image;
  private Transform iconTr;
  private Transform icons;
  private GameObject skillIcon;
  private bool cooling = false;
  private bool activating = false;
  private bool rotating = false;
  private Quaternion originalRotation;

  void Awake() {
    sCollider = GetComponent<Collider>();
    image = transform.Find("Cooling").GetComponent<Image>();
    iconTr = transform.Find("Icon");
    icons = transform.parent.Find("Icons");
  }

  public void setSkill(Skill skill) {
    this.skill = skill;
    what = skill.name;
    duration = skill.duration;
    coolTime = skill.coolTime;

    skillIcon = icons.Find(what).gameObject;
    skillIcon.transform.SetParent(iconTr, false);
    skillIcon.transform.localPosition = Vector3.zero;
    skillIcon.SetActive(true);

    originalRotation = skillIcon.transform.rotation;
    cooling = true;
  }

  void Update() {
    if (cooling) {
      image.fillAmount = Mathf.MoveTowards(image.fillAmount, 0, Time.deltaTime / coolTime);
      if (image.fillAmount == 0) {
        cooling = false;
        sCollider.enabled = true;
        rotating = true;
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
      }
    }

    if (rotating) {
      skillIcon.transform.Rotate(-Vector3.up * Time.deltaTime * 80);
    }

    if (activating) {
      image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1, Time.deltaTime / duration);
      if (image.fillAmount == 1) {
        deactivate();
      }
    }
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    skill.activate(true);
    sCollider.enabled = false;
    activating = true;
    rotating = false;
    skillIcon.transform.rotation = originalRotation;
  }

  void deactivate() {
    activating = false;
    cooling = true;
    skill.activate(false);
  }

  void OnDisble() {
    deactivate();
  }
}
