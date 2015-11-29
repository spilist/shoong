using UnityEngine;
using System.Collections;

public class FixPosition : MonoBehaviour {
  private Vector3 position;
  public bool moveUp = true;
  public float moveAmount;

	void OnEnable() {
    position = transform.position;
  }

  void Update() {
    if (moveUp) {
      position += new Vector3(0, 0, moveAmount);
    }
    transform.position = position;
  }
}
