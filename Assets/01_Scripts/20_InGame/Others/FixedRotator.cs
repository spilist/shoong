using UnityEngine;
using System.Collections;

public class FixedRotator : MonoBehaviour {
  public int speed = 300;
  public bool clockwise = true;
  public bool shrinking = true;
  public float shrinkingDuration = 0.5f;

  Vector3 rotation;
  float scale;

	void OnEnable() {
    if (clockwise) rotation = new Vector3(0, 1, 0);
    else rotation = new Vector3(0, -1, 0);

    scale = 1;
    transform.localScale = Vector3.one;
  }

  void Update () {
    transform.Rotate(rotation * Time.deltaTime * speed);

    if (shrinking) {
      scale = Mathf.MoveTowards(scale, 0, Time.deltaTime / shrinkingDuration);
      transform.localScale = scale * Vector3.one;
    }
	}
}
