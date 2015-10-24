using UnityEngine;
using System.Collections;

public class CharacterDebrisMover : MonoBehaviour {
	float scale;
  float targetScale;
  float diff;
  bool large;
  float duration;
  Rigidbody rb;

  void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  void OnEnable () {
    int speed = Random.Range(ScoreManager.sm.minDebrisSpeed, ScoreManager.sm.maxDebrisSpeed);

    rb.velocity = Random.onUnitSphere * speed;
    rb.angularVelocity = Random.onUnitSphere * ScoreManager.sm.debrisTumble;

    large = Random.Range(0, 100) > 50;

    if (large) {
      targetScale = Random.Range(ScoreManager.sm.minSizeAfterBreak, ScoreManager.sm.maxSizeAfterBreak);
      duration = ScoreManager.sm.destroyLargeAfter;
    } else {
      targetScale = 0;
      duration = ScoreManager.sm.destroySmallAfter;
    }

    diff = Mathf.Abs(targetScale - scale);
    scale = 1;
  }

  void Update () {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * diff / duration);
    transform.localScale = scale * Vector3.one;

    if ( (large && scale >= targetScale) || (!large && scale <= targetScale)) gameObject.SetActive(false);
  }
}
