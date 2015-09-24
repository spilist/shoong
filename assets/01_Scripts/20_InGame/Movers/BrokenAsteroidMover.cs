using UnityEngine;
using System.Collections;

public class BrokenAsteroidMover : MonoBehaviour {
  AsteroidManager asm;
  float scale;
  float targetScale;
  float diff;
  bool large;
  float duration;

	void Start () {
    asm = GameObject.Find("Field Objects").GetComponent<AsteroidManager>();

    int speed = Random.Range(asm.minBrokenSpeed, asm.maxBrokenSpeed);

    GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * asm.brokenTumble;

    scale = transform.localScale.x;
    large = Random.Range(0, 100) > 50;

    if (large) {
      targetScale = 15;
      duration = 1f;
    } else {
      targetScale = 0;
      duration = 4f;
    }

    diff = Mathf.Abs(targetScale - scale);
	}

	void Update () {
    scale = Mathf.MoveTowards(scale, targetScale, Time.deltaTime * diff / duration);
    transform.localScale = scale * Vector3.one;

    if ( (large && scale >= targetScale) || (!large && scale <= targetScale)) Destroy(gameObject);
	}
}
