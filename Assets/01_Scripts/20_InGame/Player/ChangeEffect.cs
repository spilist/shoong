using UnityEngine;
using System.Collections;

public class ChangeEffect : MonoBehaviour {
  private CharacterChangeManager cm;

  public string changeTo;
  public string effectName;
  public float targetScale;

  private float originalScale;
  private float scale;

  void OnEnable() {
    cm = Player.pl.GetComponent<CharacterChangeManager>();

    if (effectName != "") {
      // Player.pl.showEffect(effectName);
    }

    if (changeTo != "") {
      cm.changeCharacterTo(changeTo);
    }

    if (targetScale != 0) {
      originalScale = transform.localScale.x;
      scale = originalScale;
    }

    if (Player.pl.isOnSuperheat()) gameObject.SetActive(false);
  }

  void Update() {
    if (targetScale != 0) {
      if (scale != targetScale) {
        scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * (originalScale - targetScale) / 0.5f);
        transform.localScale = scale * Vector3.one;
      }
    }
  }

  void OnDisable() {
    if (changeTo != "") {
      cm.changeCharacterToOriginal();
    }

    if (targetScale != 0) {
      transform.localScale = originalScale * Vector3.one;
    }
  }
}
