using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillButton : MonoBehaviour {
  public string what;
  public GameObject skill;
  public float skillDuration;
  public float coolTime;

  private Collider sCollider;
  private Image image;
  private bool cooling = false;

  void Awake() {
    sCollider = GetComponent<Collider>();
    image = GetComponent<Image>();
    cooling = true;
  }

  void Update() {
    if (cooling) {
      image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1, Time.deltaTime / coolTime);
      if (image.fillAmount == 1) {
        cooling = false;
        sCollider.enabled = true;
      }
    }
  }

  void OnPointerDown() {
    if (Player.pl.uncontrollable()) return;

    Player.pl.effectedBy(what);

    skill.SetActive(true);
    Invoke("inactivate", skillDuration);
  }

  void inactivate() {
    cooling = true;
    image.fillAmount = 0;
    skill.SetActive(false);
    Player.pl.effectedBy(what, false);
  }

  void OnDisble() {
    CancelInvoke();
  }
}
