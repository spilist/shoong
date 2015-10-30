using UnityEngine;
using System.Collections;

public class RhythmRing : MonoBehaviour {
  public bool skillRing = false;
  float beat;
  float startScale;
  float scale;
  float boosterOkScale;
  bool msgSended = false;

  void Awake() {
    startScale = transform.localScale.x;
  }

  void OnEnable() {
    transform.localScale = startScale * Vector3.one;
    scale = startScale;
    msgSended = false;
    beat = RhythmManager.rm.samplePeriod;
    boosterOkScale = RhythmManager.rm.boosterOkScale;
  }

  void Update() {
    scale = Mathf.MoveTowards(scale, 0, Time.deltaTime * startScale / beat);
    transform.localScale = scale * Vector3.one;
    if (!msgSended && scale <= boosterOkScale) {
      msgSended = true;
      RhythmManager.rm.boosterOk(true, skillRing);
    }

    if (scale == 0) {
      gameObject.SetActive(false);
    }
  }

  void OnDisable() {
    RhythmManager.rm.boosterOk(false, false);
  }
}
