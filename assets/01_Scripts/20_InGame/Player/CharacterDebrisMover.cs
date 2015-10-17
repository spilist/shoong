using UnityEngine;
using System.Collections;

public class CharacterDebrisMover : MonoBehaviour {
  ScoreManager scoreManager;
	float scale;
  float targetScale;
  float diff;
  bool large;
  float duration;

  void Start () {
    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

    int speed = Random.Range(scoreManager.minDebrisSpeed, scoreManager.maxDebrisSpeed);

    GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * scoreManager.debrisTumble;

    scale = transform.localScale.x;
    large = Random.Range(0, 100) > 50;

    if (large) {
      targetScale = Random.Range(scoreManager.minSizeAfterBreak, scoreManager.maxSizeAfterBreak);
      duration = scoreManager.destroyLargeAfter;
    } else {
      targetScale = 0;
      duration = scoreManager.destroySmallAfter;
    }

    diff = Mathf.Abs(targetScale - scale);
  }

  void Update () {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * diff / duration);
    transform.localScale = scale * Vector3.one;

    if ( (large && scale >= targetScale) || (!large && scale <= targetScale)) Destroy(gameObject);
  }
}
