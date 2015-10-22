using UnityEngine;
using System.Collections;

public class BrokenAsteroidMover : MonoBehaviour {
  AsteroidManager asm;
  float scale;
  float targetScale;
  float diff;
  bool large;
  float duration;

	void Awake () {
    asm = GameObject.Find("Field Objects").GetComponent<AsteroidManager>();
  }

  void OnEnable() {
    int speed = Random.Range(asm.minBrokenSpeed, asm.maxBrokenSpeed);

    GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * asm.brokenTumble;

    large = Random.Range(0, 100) > 50;

    if (large) {
      targetScale = Random.Range(asm.minSizeAfterBreak, asm.maxSizeAfterBreak);
      duration = asm.destroyLargeAfter;
    } else {
      targetScale = 0;
      duration = asm.destroySmallAfter;
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
