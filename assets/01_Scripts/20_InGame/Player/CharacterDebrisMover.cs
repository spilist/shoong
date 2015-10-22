using UnityEngine;
using System.Collections;

public class CharacterDebrisMover : MonoBehaviour {
  ScoreManager scoreManager;
	float scale;
  float targetScale;
  float diff;
  bool large;
  float duration;
  Rigidbody rb;

  void Awake() {
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    rb = GetComponent<Rigidbody>();
  }

  void OnEnable () {
    int speed = Random.Range(scoreManager.minDebrisSpeed, scoreManager.maxDebrisSpeed);

    rb.velocity = Random.onUnitSphere * speed;
    rb.angularVelocity = Random.onUnitSphere * scoreManager.debrisTumble;

    large = Random.Range(0, 100) > 50;

    if (large) {
      targetScale = Random.Range(scoreManager.minSizeAfterBreak, scoreManager.maxSizeAfterBreak);
      duration = scoreManager.destroyLargeAfter;
    } else {
      targetScale = 0;
      duration = scoreManager.destroySmallAfter;
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
