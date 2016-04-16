using UnityEngine;
using System.Collections;

public class FixedRotator : MonoBehaviour {
  public string axis = "y";
  public int speed = 300;
  public bool clockwise = true;
  public bool shrinking = true;
  public float shrinkingDuration = 0.5f;

  Vector3 rotation;
  float originalScale;
  float scale;

  void Awake() {
    originalScale = transform.localScale.x;
  }

	void OnEnable() {
    if (axis == "x") {
      rotation = new Vector3(1, 0, 0);
    } else if (axis == "y") {
      rotation = new Vector3(0, 1, 0);
    } else if (axis == "z") {
      rotation = new Vector3(0, 0, 1);
    }

    if (!clockwise) rotation = -rotation;

    scale = originalScale;
    transform.localScale = scale * Vector3.one;
  }

  void Update () {
    transform.Rotate(rotation * Time.deltaTime * speed);

    if (shrinking) {
      scale = Mathf.MoveTowards(scale, 0, Time.deltaTime / shrinkingDuration);
      transform.localScale = scale * Vector3.one;
    }
	}
}
